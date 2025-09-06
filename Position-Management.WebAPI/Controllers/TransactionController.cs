using Microsoft.AspNetCore.Mvc;
using Position_Management.DataRepository;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class TransactionController : ControllerBase
{
    private readonly TransactionRepository _repository;

    public TransactionController(TransactionRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var transactions = await _repository.GetAllAsync();
        return Ok(transactions);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var transaction = await _repository.GetByIdAsync(id);
        if (transaction == null) return NotFound();
        return Ok(transaction);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Transaction transaction)
    {
        var version = await _repository.GetVersionByInsertUpdateCancelAsync(transaction);
        transaction.Version = version ?? 1;
        var teadeId = await _repository.GetTradeIdAsync(transaction);        
        transaction.TradeID = teadeId ?? 1;
        
        var created = await _repository.AddAsync(transaction);
        return CreatedAtAction(nameof(Get), new { id = created.TransactionID }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Transaction transaction)
    {
        if (id != transaction.TransactionID) return BadRequest();
        await _repository.UpdateAsync(transaction);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _repository.DeleteAsync(id);
        return NoContent();
    }

    [HttpGet("positions")]
    public async Task<IActionResult> GetPositions()
    {
        var transactions = await _repository.GetAllAsync();

        // Group transactions by TradeID and select the latest version for each Trade
        var latestTransactions = transactions
            .GroupBy(t => t.TradeID)
            .Select(g =>
            {
                // CANCEL is always the last version, otherwise take the highest version
                var cancelTx = g.OrderByDescending(t => t.Version)
                                .FirstOrDefault(t => t.InsertUpdateCancel == "CANCEL");
                if (cancelTx != null)
                    return cancelTx;

                return g.OrderByDescending(t => t.Version).First();
            })
            .Where(t => t.InsertUpdateCancel != "CANCEL") // Ignore cancelled trades for position
            .ToList();

        // Calculate positions
        var positions = new Dictionary<string, int>();
        foreach (var tx in latestTransactions)
        {
            int sign = tx.BuySell == "Buy" ? 1 : -1;
            if (!positions.ContainsKey(tx.SecurityCode))
                positions[tx.SecurityCode] = 0;
            positions[tx.SecurityCode] += sign * tx.Quantity;
        }

        // Format output as list for easier consumption
        var output = positions.Select((p, index) => new { id = index,SecurityCode = p.Key, Position = p.Value }).ToList();

        return Ok(output);
    }
}
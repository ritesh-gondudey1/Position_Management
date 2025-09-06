import React, { useEffect, useState } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';

type Transaction = {
    transactionID: number;
    tradeID: number;
    version: number;
    securityCode: string;
    quantity: number;
    insertUpdateCancel: string;
    buySell: string;
};

const TransactionPage: React.FC = () => {
    const [transactions, setTransactions] = useState<Transaction[]>([]);
    const [form, setForm] = useState<Omit<Transaction, 'transactionID'>>({
        tradeID: 0,
        version: 1,
        securityCode: '',
        quantity: 0,
        insertUpdateCancel: 'INSERT',
        buySell: 'Buy',
    });

    useEffect(() => {
        fetch('https://localhost:7075/api/transaction')
            .then(res => res.json())
            .then(setTransactions);
    }, []);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
        setForm({ ...form, [e.target.name]: e.target.value });
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        await fetch('https://localhost:7075/api/transaction', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(form),
        });
        fetch('https://localhost:7075/api/transaction')
            .then(res => {
                if (!res.ok) throw new Error('Network response was not ok');
                return res.json();
            })
            .then(setTransactions)
            .catch(err => {
                console.error('Fetch error:', err);
            });
    };

    const handleDelete = async (id: number) => {
        await fetch(`https://localhost:7075/api/transaction/${id}`, { method: 'DELETE' });
        setTransactions(transactions.filter(t => t.transactionID !== id));
    };

    return (
        <div className="container py-4">
            <h2 className="mb-4 text-primary">Transactions</h2>
            <form className="row g-3 mb-4 bg-light p-3 rounded shadow-sm" onSubmit={handleSubmit}>
                {/*<div className="col-md-2">*/}
                {/*    <input className="form-control" name="tradeID" type="number" value={form.tradeID} onChange={handleChange} placeholder="Trade ID" required />*/}
                {/*</div>*/}
                {/*<div className="col-md-2">*/}
                {/*    <input className="form-control" name="version" type="number" value={form.version} onChange={handleChange} placeholder="Version" required />*/}
                {/*</div>*/}
                <div className="col-md-3">
                    <input className="form-control" name="securityCode" value={form.securityCode} onChange={handleChange} placeholder="Security Code" required />
                </div>
                <div className="col-md-2">
                    <input className="form-control" name="quantity" type="number" value={form.quantity} onChange={handleChange} placeholder="Quantity" required />
                </div>
                <div className="col-md-2">
                    <select className="form-select" name="insertUpdateCancel" value={form.insertUpdateCancel} onChange={handleChange}>
                        <option value="INSERT">INSERT</option>
                        <option value="UPDATE">UPDATE</option>
                        <option value="CANCEL">CANCEL</option>
                    </select>
                </div>
                <div className="col-md-2">
                    <select className="form-select" name="buySell" value={form.buySell} onChange={handleChange}>
                        <option value="Buy">Buy</option>
                        <option value="Sell">Sell</option>
                    </select>
                </div>
                <div className="col-3">
                    <button type="submit" className="btn btn-success">Add Transaction</button>
                </div>
            </form>
            <div className="table-responsive">
                <table className="table table-striped table-bordered align-middle shadow-sm">
                    <thead className="table-primary">
                        <tr>
                            <th>ID</th>
                            <th>TradeID</th>
                            <th>Version</th>
                            <th>SecurityCode</th>
                            <th>Quantity</th>
                            <th>Insert/Update/Cancel</th>
                            <th>Buy/Sell</th>
                            <th>Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        {transactions.map(t => (
                            <tr key={t.transactionID}>
                                <td>{t.transactionID}</td>
                                <td>{t.tradeID}</td>
                                <td>{t.version}</td>
                                <td>{t.securityCode}</td>
                                <td>{t.quantity}</td>
                                <td>{t.insertUpdateCancel}</td>
                                <td>{t.buySell}</td>
                                <td>
                                    <button className="btn btn-danger btn-sm" onClick={() => handleDelete(t.transactionID)}>
                                        Delete
                                    </button>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>
        </div>
    );
};

export default TransactionPage;
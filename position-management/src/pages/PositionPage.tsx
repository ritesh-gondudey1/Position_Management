import React, { useEffect, useState } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';

type Position = {
    id: number;
    SecurityCode: string;
    Position: number;
};

const PositionPage: React.FC = () => {
    const [positions, setPositions] = useState<Position[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        fetch('https://localhost:7075/api/transaction/positions')
            .then(res => res.json())
            .then(data => {
                setPositions(data);
                setLoading(false);
            })
            .catch(() => setLoading(false));
    }, []);

    return (
        <div className="container py-4">
            <h2 className="mb-4 text-primary" style={{ fontStyle: 'italic' }}>Position</h2>
            {loading ? (
                <div className="text-center py-5">
                    <div className="spinner-border text-primary" role="status">
                        <span className="visually-hidden">Loading...</span>
                    </div>
                </div>
            ) : (
                <table className="table table-bordered w-auto" style={{ maxWidth: 300 }}>
                    <tbody>
                        {positions.map(pos => (
                            <tr key={pos.id}>
                                <td><strong>{pos.SecurityCode}</strong></td>
                                <td>{pos.Position > 0 ? `+${pos.Position}` : pos.Position}</td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            )}
        </div>
    );
};

export default PositionPage;
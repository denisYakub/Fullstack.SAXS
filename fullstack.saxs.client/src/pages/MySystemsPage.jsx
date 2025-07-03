import React, { useEffect, useState } from 'react';
import { getMyGenerations } from '../api/systemApi';
import { useNavigate } from 'react-router-dom';

export default function MySystemsPage() {
    const [gens, setGens] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const navigate = useNavigate();

    useEffect(() => {
        getMyGenerations()
            .then(data => {
                setGens(data);
                setLoading(false);
            })
            .catch(err => {
                if (err.message === 'Unauthorized') {
                    window.location.href = 'https://localhost:7135/Identity/Account/Login';
                    return;
                }
                setError(err.message || 'Error loading');
                setLoading(false);
            });
    }, []);

    if (loading) return <div>Loading...</div>;
    if (error) return <div style={{ color: 'red' }}>Error: {error}</div>;

    return (
        <div>
            <h2>My Systems</h2>
            <table border="1" cellPadding="8" style={{ borderCollapse: 'collapse', width: '100%' }}>
                <thead>
                    <tr>
                        <th>GenNum</th>
                        <th>SeriesNum</th>
                        <th>AreaType</th>
                        <th>ParticleType</th>
                        <th>Phi</th>
                        <th>ParticleNum</th>
                        <th>IdSpData</th>
                    </tr>
                </thead>
                <tbody>
                    {gens.map(g => (
                        <tr
                            key={g.IdSpData}
                            style={{ cursor: 'pointer' }}
                            onClick={() => navigate(`/system/${g.IdSpData}`)}
                        >
                            <td>{g.GenNum}</td>
                            <td>{g.SeriesNum}</td>
                            <td>{g.AreaType}</td>
                            <td>{g.ParticleType}</td>
                            <td>{g.Phi}</td>
                            <td>{g.ParticleNum}</td>
                            <td>{g.IdSpData}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
}

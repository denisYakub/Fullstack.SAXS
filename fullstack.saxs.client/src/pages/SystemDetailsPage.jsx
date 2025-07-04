import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { getSystem } from '../api/SystemApi';
import SysView from '../components/SysView';

export default function SystemDetailsPage() {
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [data, setData] = useState(null);

    const { id } = useParams();

    useEffect(() => {
        if (!id) return;

        getSystem(id)
            .then(setData)
            .then(setLoading(false))
            .catch(err => {
                setError(err.message || 'Error loading');
                setLoading(false);
            });
    }, [id]);

    if (loading) return <div>Loading...</div>;
    if (error) return <div style={{ color: 'red' }}>Error: {error}</div>;

    return (
        <div>
            <h2>System Details</h2>
            <p>ID system: {id}</p>

            {data && (
                <div style={{ height: '100vh', width: '100vh' }}>
                    <SysView data={data} />
                </div>
            )}
        </div>
    );
}

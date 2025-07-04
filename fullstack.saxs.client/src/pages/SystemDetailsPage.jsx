import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { getSystem } from '../api/SystemApi';
import SysView from '../components/SysView';

export default function SystemDetailsPage() {
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [data, setData] = useState(null);

    const { id } = useParams();

    function openPhiGraph(id) {
        const form = document.createElement('form');
        form.method = 'POST';
        form.action = `/api/saxs/systems/${id}/graphs/phi`;
        form.target = '_blank';

        const input = document.createElement('input');
        input.type = 'hidden';
        input.name = 'layersNum';
        input.value = '5';

        form.appendChild(input);
        document.body.appendChild(form);
        form.submit();
        document.body.removeChild(form);
    }

    useEffect(() => {
        if (!id) return;

        setLoading(true);
        getSystem(id)
            .then(systemData => {
                setData(systemData);
                setLoading(false);
            })
            .catch(err => {
                setError(err.message || 'Loading Error');
                setLoading(false);
            });
    }, [id]);

    if (loading)
        return (
            <div className="flex justify-center items-center min-h-screen text-xl text-gray-400">
                Loading...
            </div>
        );

    if (error)
        return (
            <div className="flex justify-center items-center min-h-screen text-red-500 font-semibold">
                Error: {error}
            </div>
        );

    return (
        <div className="min-h-screen flex flex-col justify-center items-center bg-gradient-to-r from-purple-800 via-indigo-900 to-blue-900 text-white px-4">
            <div className="w-[50vw] mx-auto p-6 space-y-6">
                <h2 className="text-3xl font-bold select-none">System Details</h2>
                <p className="text-gray-300">ID system: <span className="font-mono">{id}</span></p>

                {data && (
                    <div className="border rounded-md shadow-md p-4" style={{ height: '60vh', width: '100%' }}>
                        <SysView data={data} />
                    </div>
                )}

                <button
                    onClick={() => openPhiGraph(id)}
                    className="px-6 py-2 bg-indigo-600 hover:bg-indigo-700 rounded-md text-white font-semibold transition"
                >
                    Open Phi graph
                </button>
            </div>
        </div>
    );
}

import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { getSystem, openPhiGraph } from '../api/SystemApi';
import { GetAtomsCoord, GetQI } from '../api/MathCadApi';
import SysView from '../components/SysView';

export default function SystemDetailsPage() {
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [data, setData] = useState(null);
    const [form, setForm] = useState({
        qMin: 0.02,
        qMax: 5,
        qNum: 150,
    });

    const requestBody = {
        QMin: Number(form.qMin),
        QMax: Number(form.qMax),
        QNum: Number(form.qNum)
    };

    const { id } = useParams();

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
        <div className="min-h-screen flex flex-col bg-gray-300 justify-center items-center text-white px-4 pt-8">
            <div className="max-w mx-auto p-6 bg-gray-600 text-white rounded-md shadow-lg mt-8">
                <div className="w-[50vw] mx-auto p-6 space-y-6">
                    <h2 className="text-3xl font-bold select-none">System Details</h2>
                    <p className="text-gray-300">ID system: <span className="font-mono">{id}</span></p>

                    {data && (
                        <div className="rounded-md shadow-md h-[60vh] w-full">
                            <SysView data={data} />
                        </div>
                    )}

                    <h3 className="text-xl font-semibold text-white mb-4 select-none">
                        System graphs
                    </h3>
                    <div className="flex flex-wrap gap-4 mt-6 justify-center">
                        <button
                            onClick={() => openPhiGraph(id)}
                            className='px-6 py-3 rounded-md transition bg-gradient-to-r from-gray-500 to-gray-700 hover:from-gray-400 hover:to-gray-600text-white'
                        >
                            Distribution density
                        </button>
                    </div>

                    <h3 className="text-xl font-semibold text-white mb-4 select-none">
                        Mathcad files
                    </h3>
                    <div className="flex flex-wrap gap-4 mt-6 justify-center">
                        <button
                            onClick={() => GetAtomsCoord(id)}
                            className='px-6 py-3 rounded-md transition bg-gradient-to-r from-gray-500 to-gray-700 hover:from-gray-400 hover:to-gray-600text-white'
                        >
                            Download atoms coordinates
                        </button>

                        <button
                            onClick={() => GetQI(requestBody)}
                            className='px-6 py-3 rounded-md transition bg-gradient-to-r from-gray-500 to-gray-700 hover:from-gray-400 hover:to-gray-600text-white'
                        >
                            Download QI vector
                        </button>
                    </div>
                </div>
            </div>
        </div>
    );
}

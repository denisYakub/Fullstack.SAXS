import React, { useEffect, useState } from 'react';
import { getAllGenerations } from '../api/systemApi';
import { useNavigate } from 'react-router-dom';

export default function AllSystemsPage() {
    const [gens, setGens] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const navigate = useNavigate();

    useEffect(() => {
        getAllGenerations()
            .then(data => {
                setGens(data);
                setLoading(false);
            })
            .catch(err => {
                setError(err.message || 'Error loading');
                setLoading(false);
            });
    }, []);

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
        <div className="min-h-screen flex flex-col justify-center items-center bg-gradient-to-r from-purple-800 via-indigo-900 to-blue-900 text-white px-4 pt-8">
            <div className="max-w-6xl bg-gray-700 mx-auto px-4 py-8">
                <h2 className="text-3xl font-bold mb-6 select-none text-center">All Systems</h2>
                <div className="overflow-x-auto">
                    <table className="min-w-full border border-gray-700 rounded-lg">
                        <thead className="bg-gray-800 text-white">
                            <tr>
                                {['GenNum', 'SeriesNum', 'AreaType', 'ParticleType', 'AreaOuterRadius', 'Phi', 'ParticleNum', 'IdSpData'].map(col => (
                                    <th
                                        key={col}
                                        className="py-3 px-4 text-left border-b border-gray-600 select-none"
                                    >
                                        {col}
                                    </th>
                                ))}
                            </tr>
                        </thead>
                        <tbody>
                            {gens.map(g => (
                                <tr
                                    key={g.IdSpData}
                                    className="cursor-pointer hover:bg-indigo-700 transition-colors border-b border-gray-700"
                                    onClick={() => navigate(`/system/${g.IdSpData}`)}
                                >
                                    <td className="py-2 px-4">{g.GenNum}</td>
                                    <td className="py-2 px-4">{g.SeriesNum}</td>
                                    <td className="py-2 px-4">{g.AreaType}</td>
                                    <td className="py-2 px-4">{g.ParticleType}</td>
                                    <td className="py-2 px-4">{g.AreaOuterRadius}</td>
                                    <td className="py-2 px-4">{g.Phi}</td>
                                    <td className="py-2 px-4">{g.ParticleNum}</td>
                                    <td className="py-2 px-4">{g.IdSpData}</td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    );
}

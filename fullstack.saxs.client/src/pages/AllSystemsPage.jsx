import React, { useEffect, useState } from 'react';
import { getAllGenerations } from '../api/systemApi';
import { useNavigate } from 'react-router-dom';
import LoadingPage from './LoadingPage';
import Toast from '../components/Toast';

export default function AllSystemsPage() {
    const [showToast, setShowToast] = useState(false);
    const [toastType, setToastType] = useState('info');
    const [message, setMessage] = useState(null);
    const [gens, setGens] = useState([]);
    const [loading, setLoading] = useState(true);
    const navigate = useNavigate();

    useEffect(() => {
        getAllGenerations()
            .then(data => {
                setGens(data);
                if (data.length <= 0) {
                    setShowToast(true);
                    setMessage('No systems yet.');
                    setToastType('warning');
                }
                setLoading(false);
            })
            .catch(err => {
                setShowToast(true);
                setMessage(err.message);
                setToastType('error');
                setLoading(false);
            });
    }, []);

    if (loading) return <LoadingPage />;

    return (
        <div className="min-h-screen flex flex-col items-center bg-gray-300 text-white px-4 pt-10 pb-10">
            <div className="w-full max-w-6xl bg-gray-700 rounded-xl shadow-2xl p-8">
                <h2 className="text-4xl font-bold mb-8 text-center select-none">All Systems</h2>

                {gens.length === 0 ? (
                    <div className="text-center text-lg text-gray-200 mt-10">No systems available.</div>
                ) : (
                    <div className="overflow-x-auto">
                        <table className="min-w-full border border-gray-600 rounded-lg overflow-hidden">
                            <thead className="bg-gray-800 text-white">
                                <tr>
                                    {[
                                        'GenNum', 'SeriesNum', 'AreaType', 'ParticleType',
                                        'AreaOuterRadius', 'Phi', 'ParticleNum'
                                    ].map(col => (
                                        <th
                                            key={col}
                                            className="px-5 py-3 text-left border-b border-gray-500 select-none"
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
                                        onClick={() => navigate(`/system/${g.IdSpData}`)}
                                        className="cursor-pointer bg-gray-600 hover:bg-gray-500 transition-colors duration-200"
                                    >
                                        <td className="px-5 py-2">{g.GenNum}</td>
                                        <td className="px-5 py-2">{g.SeriesNum}</td>
                                        <td className="px-5 py-2">{g.AreaType}</td>
                                        <td className="px-5 py-2">{g.ParticleType}</td>
                                        <td className="px-5 py-2">{g.AreaOuterRadius}</td>
                                        <td className="px-5 py-2">{g.Phi}</td>
                                        <td className="px-5 py-2">{g.ParticleNum}</td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    </div>
                )}
            </div>

            {showToast && message && (
                <Toast message={message} type={toastType} />
            )}
        </div>
    );
}

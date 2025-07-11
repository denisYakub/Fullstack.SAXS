import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { getSystem, openPhiGraph } from '../api/SystemApi';
import { GetAtomsCoord, GetQI } from '../api/MathCadApi';
import SysView from '../components/SysView';
import LoadingPage from './LoadingPage';
import Toast from '../components/Toast';

export default function SystemDetailsPage() {
    const [showToast, setShowToast] = useState(false);
    const [toastType, setToastType] = useState('info');
    const [message, setMessage] = useState(null);
    const [loading, setLoading] = useState(true);
    const [data, setData] = useState(null);
    const [formQ, setFormQ] = useState({
        qMin: 0.02,
        qMax: 5,
        qNum: 150,
    });
    const [formPhi, setFormPhi] = useState({
        phiNumber: 5,
    });

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
                setShowToast(true);
                setMessage(err.message);
                setToastType('error');
                setLoading(false);
            });
    }, [id]);

    function onChangePhi(e) {
        const { name, value } = e.target;
        setFormPhi(f => ({ ...f, [name]: value }));
    }

    function onChangeQ(e) {
        const { name, value } = e.target;
        setFormQ(f => ({ ...f, [name]: value }));
    }

    async function onSubmitDistributionDensity(e) {
        e.preventDefault();
        openPhiGraph(id, formPhi.phiNumber)
    }

    async function onSubmitDownloadAtomsCoordinates(e) {
        e.preventDefault();
        GetAtomsCoord(id);
    }

    async function onSubmitDownloadQIVector(e) {
        e.preventDefault();

        const requestBody = {
            QMin: Number(formQ.qMin),
            QMax: Number(formQ.qMax),
            QNum: Number(formQ.qNum)
        };

        GetQI(requestBody);
    }

    if (loading) return <LoadingPage />;

    return (
        <div className="min-h-screen flex flex-col bg-gray-300 justify-center items-center text-white px-4 pt-8">
            <div className="max-w-5xl w-full mx-auto p-8 bg-gray-700 text-white rounded-lg shadow-xl">
                <h2 className="text-4xl font-bold mb-4 select-none text-center">System Details</h2>

                <div className="text-sm text-gray-300 mb-6 text-center">
                    <p className="mb-1">
                        ID system: <span className="font-mono">{id}</span>
                    </p>
                    <p>
                        Particles type: <span className="font-mono">{data.getFirstParticleTypeName()}</span>
                    </p>
                </div>

                {data && (
                    <div className="rounded-md shadow-md h-[60vh] w-full mb-8 border border-gray-500">
                        <SysView data={data} />
                    </div>
                )}

                {/* Секция графиков */}
                <div className="mb-10">
                    <h3 className="text-2xl font-semibold mb-4 select-none border-b border-gray-500 pb-2">System Graphs</h3>
                    <form onSubmit={onSubmitDistributionDensity} className="space-y-4">
                        <label className="block">
                            <span className="mb-1 block font-semibold">phiNumber:</span>
                            <input
                                type="number"
                                step="any"
                                name="phiNumber"
                                value={formPhi["phiNumber"]}
                                onChange={onChangePhi}
                                required
                                className="w-full rounded-md bg-gray-600 px-3 py-2 text-white focus:outline-none focus:ring-2 focus:ring-indigo-400"
                            />
                        </label>
                        <button
                            type="submit"
                            className="w-full py-3 rounded-md bg-indigo-600 hover:bg-indigo-700 transition text-white font-semibold"
                        >
                            Generate Distribution Density
                        </button>
                    </form>
                </div>

                {/* Секция Mathcad */}
                <div className="mb-10">
                    <h3 className="text-2xl font-semibold mb-4 select-none border-b border-gray-500 pb-2">Mathcad Files</h3>
                    <form onSubmit={onSubmitDownloadQIVector} className="space-y-4">
                        {['qMin', 'qMax', 'qNum'].map(name => (
                            <label key={name} className="block">
                                <span className="mb-1 block font-semibold">{name}:</span>
                                <input
                                    type="number"
                                    step="any"
                                    name={name}
                                    value={formQ[name]}
                                    onChange={onChangeQ}
                                    required
                                    className="w-full rounded-md bg-gray-600 px-3 py-2 text-white focus:outline-none focus:ring-2 focus:ring-indigo-400"
                                />
                            </label>
                        ))}
                        <button
                            type="submit"
                            className="w-full py-3 rounded-md bg-emerald-600 hover:bg-emerald-700 transition text-white font-semibold"
                        >
                            Download QI Vector
                        </button>
                    </form>
                </div>

                {/* Кнопка загрузки координат */}
                <div className="text-center">
                    <button
                        onClick={onSubmitDownloadAtomsCoordinates}
                        className="px-6 py-3 rounded-md bg-orange-500 hover:bg-orange-600 transition text-white font-semibold"
                    >
                        Download Atoms Coordinates
                    </button>
                </div>
            </div>

            {/* Toast уведомление */}
            {showToast && message && <Toast message={message} type={toastType} />}
        </div>
    );
}

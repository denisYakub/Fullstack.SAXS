import React, { useState } from 'react';
import { createSystem } from '../api/systemApi';
import { DrawAllParticleTypes } from '../components/DrawParticle';
import Toast from '../components/Toast';

const PARTICLE_TYPES = ['Icosahedron', 'C60', 'C70', 'C240', 'C540'];

export default function CreateSystemPage() {
    const [showToast, setShowToast] = useState(false);
    const [toastType, setToastType] = useState('info');
    const [form, setForm] = useState({
        AreaRadius: '',
        Nc: '',
        ParticleType: 'Icosahedron',
        ParticleMinSize: '',
        ParticleMaxSize: '',
        ParticleAlphaRotation: '360',
        ParticleBetaRotation: '360',
        ParticleGammaRotation: '360',
        ParticleSizeShape: '3',
        ParticleSizeScale: '2.5',
        ParticleNumber: '',
        AreaNumber: '',
        IsLegit: true,
    });
    const [loading, setLoading] = useState(false);
    const [message, setMessage] = useState('');

    function onChange(e) {
        const { name, value } = e.target;
        setForm(f => ({ ...f, [name]: value }));
    }

    function isValidRequest() {
        return (form.AreaRadius.trim() !== '' || form.Nc.trim() !== '') && form.IsLegit;
    }

    async function onSubmit(e) {
        e.preventDefault();
        if (!isValidRequest()) {
            setMessage('Fill in either AreaRadius or Nc');
            setShowToast(true);
            setToastType('warning');
            return;
        }

        setLoading(true);
        setMessage('');

        const requestBody = {
            AreaRadius: form.AreaRadius ? Number(form.AreaRadius) : null,
            Nc: form.Nc ? Number(form.Nc) : null,
            ParticleType: form.ParticleType,
            ParticleMinSize: Number(form.ParticleMinSize),
            ParticleMaxSize: Number(form.ParticleMaxSize),
            ParticleAlphaRotation: Number(form.ParticleAlphaRotation),
            ParticleBetaRotation: Number(form.ParticleBetaRotation),
            ParticleGammaRotation: Number(form.ParticleGammaRotation),
            ParticleSizeShape: Number(form.ParticleSizeShape),
            ParticleSizeScale: Number(form.ParticleSizeScale),
            ParticleNumber: Number(form.ParticleNumber),
            AreaNumber: Number(form.AreaNumber),
            IsLegit: true,
        };

        try {
            await createSystem(requestBody);
            setMessage('System creation complete.');
            setShowToast(true);
            setToastType('success');
            setForm({
                AreaRadius: '',
                Nc: '',
                ParticleType: 'Icosahedron',
                ParticleMinSize: '',
                ParticleMaxSize: '',
                ParticleAlphaRotation: '360',
                ParticleBetaRotation: '360',
                ParticleGammaRotation: '360',
                ParticleSizeShape: '3',
                ParticleSizeScale: '2.5',
                ParticleNumber: '',
                AreaNumber: '',
                IsLegit: true,
            });
        } catch (error) {
            setMessage('Error: ' + error.message);
            setShowToast(true);
            setToastType('error');
        } finally {
            setLoading(false);
        }
    }

    return (
        <div className="min-h-screen flex items-center justify-center bg-gray-300 px-4 pt-10 text-white">
            <div className="flex flex-row gap-10">
                {/* Левая часть: форма */}
                <div className="w-1/2 p-8 bg-gray-700 rounded-lg shadow-xl">
                    <h2 className="text-4xl font-bold mb-6 text-center select-none">Create System</h2>
                    <form onSubmit={onSubmit} className="space-y-5">
                        {/* Выпадающий список */}
                        <label className="block">
                            <span className="block font-semibold mb-1">Particle Type:</span>
                            <select
                                name="ParticleType"
                                value={form.ParticleType}
                                onChange={onChange}
                                className="w-full rounded-md bg-gray-600 px-3 py-2 text-white focus:outline-none focus:ring-2 focus:ring-indigo-400"
                            >
                                {PARTICLE_TYPES.map((type) => (
                                    <option key={type} value={type}>{type}</option>
                                ))}
                            </select>
                        </label>

                        {/* Инпуты */}
                        {[
                            { label: 'AreaRadius (or leave blank if filling Nc)', name: 'AreaRadius', required: false, step: 'any' },
                            { label: 'Nc (or leave blank if you fill in AreaRadius)', name: 'Nc', required: false, step: 'any' },
                            { label: 'ParticleMinSize', name: 'ParticleMinSize', required: true, step: 'any' },
                            { label: 'ParticleMaxSize', name: 'ParticleMaxSize', required: true, step: 'any' },
                            { label: 'ParticleAlphaRotation', name: 'ParticleAlphaRotation', required: true, step: 'any' },
                            { label: 'ParticleBetaRotation', name: 'ParticleBetaRotation', required: true, step: 'any' },
                            { label: 'ParticleGammaRotation', name: 'ParticleGammaRotation', required: true, step: 'any' },
                            { label: 'ParticleSizeShape', name: 'ParticleSizeShape', required: true, step: 'any' },
                            { label: 'ParticleSizeScale', name: 'ParticleSizeScale', required: true, step: 'any' },
                            { label: 'ParticleNumber', name: 'ParticleNumber', required: true },
                            { label: 'AreaNumber', name: 'AreaNumber', required: true },
                        ].map(({ label, name, required, step }) => (
                            <label key={name} className="block">
                                <span className="block font-semibold mb-1">{label}:</span>
                                <input
                                    type="number"
                                    step={step}
                                    name={name}
                                    value={form[name]}
                                    onChange={onChange}
                                    required={required}
                                    className="w-full rounded-md bg-gray-600 px-3 py-2 text-white focus:outline-none focus:ring-2 focus:ring-indigo-400"
                                    placeholder={required ? 'Required' : ''}
                                />
                            </label>
                        ))}

                        {/* Кнопка */}
                        <button
                            type="submit"
                            disabled={loading}
                            className={`w-full py-3 rounded-md text-white font-semibold transition
                            ${loading
                                    ? 'bg-gradient-to-r from-gray-600 to-gray-700 cursor-not-allowed'
                                    : 'bg-gradient-to-r from-indigo-500 to-indigo-700 hover:from-indigo-400 hover:to-indigo-600'
                                }`}
                        >
                            {loading ? 'Creating...' : 'Create a system'}
                        </button>
                    </form>

                    {/* Toast уведомление */}
                    {showToast && message && <Toast message={message} type={toastType} />}
                </div>

                {/* Правая часть: визуализация */}
                <DrawAllParticleTypes selectedType={form.ParticleType} />
            </div>
        </div>
    );

}

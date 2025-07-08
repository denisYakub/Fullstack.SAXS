import React, { useState } from 'react';
import { createSystem } from '../api/systemApi';
import { DrawAllParticleTypes } from '../components/DrawParticle';

const PARTICLE_TYPES = ['Icosahedron', 'C60', 'C70', 'C240', 'C540'];

export default function CreateSystemPage() {
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
        } finally {
            setLoading(false);
        }
    }

    return (
        <div className="min-h-screen flex flex-row justify-center items-center bg-gray-300  text-white px-4 pt-8">
            <div className="flex flex-row gap-[10px]">
                <div className="p-6 bg-gray-600 text-white rounded-md shadow-lg">
                    <h2 className="text-3xl font-bold mb-6 text-center select-none">Create System</h2>
                    <form onSubmit={onSubmit} className="space-y-4">
                        {/* ¬€œ¿ƒ¿ﬁŸ»… —œ»—Œ  */}
                        <label className="block">
                            <span className="mb-1 block font-semibold">Particle Type:</span>
                            <select
                                name="ParticleType"
                                value={form.ParticleType}
                                onChange={onChange}
                                className="w-full rounded-md bg-gray-700 px-3 py-2 text-white placeholder-white/50 focus:border-indigo-400 focus:outline-none"
                            >
                                {PARTICLE_TYPES.map((type) => (
                                    <option key={type} value={type}>{type}</option>
                                ))}
                            </select>
                        </label>

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
                            { label: 'ParticleNumber', name: 'ParticleNumber', required: true, step: undefined },
                            { label: 'AreaNumber', name: 'AreaNumber', required: true, step: undefined },
                        ].map(({ label, name, required, step }) => (
                            <label key={name} className="block">
                                <span className="mb-1 block font-semibold">{label}:</span>
                                <input
                                    type="number"
                                    step={step}
                                    name={name}
                                    value={form[name]}
                                    onChange={onChange}
                                    required={required}
                                    className="w-full rounded-md bg-gray-700 px-3 py-2 text-white placeholder-white/50 focus:border-indigo-400 focus:outline-none"
                                    placeholder={required ? 'Required' : ''}
                                />
                            </label>
                        ))}

                        <button
                            type="submit"
                            disabled={loading}
                            className={
                                `w-full py-3 rounded-md
                                ${loading ?
                                    'bg-gradient-to-r from-gray-600 to-gray-700 cursor-not-allowed' :
                                    'bg-gradient-to-r from-gray-500 to-gray-700 hover:from-gray-400 hover:to-gray-600'}
                                text-white`
                            }>
                            {loading ? 'Creating...' : 'Create a system'}
                        </button>

                    </form>

                    {message && (
                        <p className={`mt-6 text-center font-medium ${message.startsWith('Error') ? 'text-red-400' : 'text-green-400'}`}>
                            {message}
                        </p>
                    )}
                </div>
                <DrawAllParticleTypes selectedType={form.ParticleType} />
            </div>
        </div>
    );
}

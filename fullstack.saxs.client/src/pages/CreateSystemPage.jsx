import React, { useState } from 'react';
import { createSystem } from '../api/systemApi';

export default function CreateSystemPage() {
    const [form, setForm] = useState({
        AreaRadius: '',
        Nc: '',
        ParticleMinSize: '',
        ParticleMaxSize: '',
        ParticleAlphaRotation: '',
        ParticleBetaRotation: '',
        ParticleGammaRotation: '',
        ParticleSizeShape: '',
        ParticleSizeScale: '',
        ParticleNumber: '',
        AreaNumber: '',
        IsLegit: true,
    });
    const [loading, setLoading] = useState(false);
    const [message, setMessage] = useState('');

    // Обработчик изменения input
    function onChange(e) {
        const { name, value } = e.target;
        setForm((f) => ({ ...f, [name]: value }));
    }

    // Проверка, чтобы AreaRadius или Nc был заполнен
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

        // Преобразуем строки в числа, учтём что либо AreaRadius, либо Nc
        const requestBody = {
            AreaRadius: form.AreaRadius ? Number(form.AreaRadius) : null,
            Nc: form.Nc ? Number(form.Nc) : null,
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
                ParticleMinSize: '',
                ParticleMaxSize: '',
                ParticleAlphaRotation: '',
                ParticleBetaRotation: '',
                ParticleGammaRotation: '',
                ParticleSizeShape: '',
                ParticleSizeScale: '',
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
        <div>
            <h2>Create System</h2>
            <form onSubmit={onSubmit}>
                <label>
                    AreaRadius (or leave blank if filling Nc):<br />
                    <input
                        type="number"
                        step="any"
                        name="AreaRadius"
                        value={form.AreaRadius}
                        onChange={onChange}
                    />
                </label>
                <br />
                <label>
                    Nc (or leave blank if you fill in AreaRadius):<br />
                    <input
                        type="number"
                        step="any"
                        name="Nc"
                        value={form.Nc}
                        onChange={onChange}
                    />
                </label>
                <br />
                <label>
                    ParticleMinSize:<br />
                    <input
                        type="number"
                        step="any"
                        name="ParticleMinSize"
                        value={form.ParticleMinSize}
                        onChange={onChange}
                        required
                    />
                </label>
                <br />
                <label>
                    ParticleMaxSize:<br />
                    <input
                        type="number"
                        step="any"
                        name="ParticleMaxSize"
                        value={form.ParticleMaxSize}
                        onChange={onChange}
                        required
                    />
                </label>
                <br />
                <label>
                    ParticleAlphaRotation:<br />
                    <input
                        type="number"
                        step="any"
                        name="ParticleAlphaRotation"
                        value={form.ParticleAlphaRotation}
                        onChange={onChange}
                        required
                    />
                </label>
                <br />
                <label>
                    ParticleBetaRotation:<br />
                    <input
                        type="number"
                        step="any"
                        name="ParticleBetaRotation"
                        value={form.ParticleBetaRotation}
                        onChange={onChange}
                        required
                    />
                </label>
                <br />
                <label>
                    ParticleGammaRotation:<br />
                    <input
                        type="number"
                        step="any"
                        name="ParticleGammaRotation"
                        value={form.ParticleGammaRotation}
                        onChange={onChange}
                        required
                    />
                </label>
                <br />
                <label>
                    ParticleSizeShape:<br />
                    <input
                        type="number"
                        step="any"
                        name="ParticleSizeShape"
                        value={form.ParticleSizeShape}
                        onChange={onChange}
                        required
                    />
                </label>
                <br />
                <label>
                    ParticleSizeScale:<br />
                    <input
                        type="number"
                        step="any"
                        name="ParticleSizeScale"
                        value={form.ParticleSizeScale}
                        onChange={onChange}
                        required
                    />
                </label>
                <br />
                <label>
                    ParticleNumber:<br />
                    <input
                        type="number"
                        name="ParticleNumber"
                        value={form.ParticleNumber}
                        onChange={onChange}
                        required
                    />
                </label>
                <br />
                <label>
                    AreaNumber:<br />
                    <input
                        type="number"
                        name="AreaNumber"
                        value={form.AreaNumber}
                        onChange={onChange}
                        required
                    />
                </label>
                <br />
                <button type="submit" disabled={loading}>
                    {loading ? 'Creating...' : 'Create a system'}
                </button>
            </form>
            {message && <p>{message}</p>}
        </div>
    );
}

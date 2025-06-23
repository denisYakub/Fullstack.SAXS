import React, { useState } from "react";

const CreateSysForm = () => {
    const [formData, setFormData] = useState({
        areaRadius: "",
        nc: "",
        particleMinSize: "",
        particleMaxSize: "",
        particleAlphaRotation: "",
        particleBetaRotation: "",
        particleGammaRotation: "",
        particleSizeShape: "",
        particleSizeScale: "",
        particleNumber: "",
        areaNumber: "",
    });

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData((prev) => ({
            ...prev,
            [name]: value,
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        const requestBody = {
            areaRadius: parseFloat(formData.areaRadius) || null,
            nc: parseFloat(formData.nc) || null,
            particleMinSize: parseFloat(formData.particleMinSize),
            particleMaxSize: parseFloat(formData.particleMaxSize),
            particleAlphaRotation: parseFloat(formData.particleAlphaRotation),
            particleBetaRotation: parseFloat(formData.particleBetaRotation),
            particleGammaRotation: parseFloat(formData.particleGammaRotation),
            particleSizeShape: parseFloat(formData.particleSizeShape),
            particleSizeScale: parseFloat(formData.particleSizeScale),
            particleNumber: parseInt(formData.particleNumber),
            areaNumber: parseInt(formData.areaNumber),
        };

        try {
            const res = await fetch("/api/saxs/sys/create", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(requestBody),
            });

            if (!res.ok) {
                throw new Error("Request failed");
            }

            alert("System created successfully!");
        } catch (error) {
            alert("Error creating system: " + error);
        }
    };

    return (
        <form onSubmit={handleSubmit} style={{ display: "grid", gap: "8px", maxWidth: "400px" }}>
            {Object.entries(formData).map(([key, value]) => (
                <div key={key}>
                    <label>{key}:</label>
                    <input type="text" name={key} value={value} onChange={handleChange} />
                </div>
            ))}
            <button type="submit">Create System</button>
        </form>
    );
};

export default CreateSysForm;

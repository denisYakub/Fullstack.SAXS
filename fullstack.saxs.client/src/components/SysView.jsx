import React, { Suspense, useState } from 'react';
import { Canvas } from '@react-three/fiber';
import { OrbitControls } from '@react-three/drei';
import ParticleMesh from './ParticleMesh';

export default function SysView({ data }) {
    const [visibleCount, setVisibleCount] = useState(1); // от 1 до 10 шагов по 10%

    const outerRadius = data.OuterRadius * 2;
    const totalParticles = data.Particles.length;
    const percent = visibleCount * 10;
    const visibleParticles = Math.floor((percent / 100) * totalParticles);

    return (
        <div className="w-full h-full relative bg-black rounded-md blur hover:blur-none transition-filter duration-300">
            {/* Ползунок */}
            <div className="absolute top-4 right-4 bg-black/50 px-4 py-2 rounded-lg z-10 text-white text-sm">
                <label>
                    Show: {visibleParticles} / {totalParticles}
                    <input
                        type="range"
                        min="1"
                        max="10"
                        value={visibleCount}
                        onChange={(e) => setVisibleCount(Number(e.target.value))}
                        className="w-36 ml-2.5 accent-orange-500"
                    />
                </label>
            </div>

            {/* Canvas в 100% */}
            <Canvas
                className="absolute top-0 left-0 w-full h-full"
                camera={{ position: [0, 0, outerRadius * 1.2], fov: 60 }}
            >
                <ambientLight />
                <directionalLight position={[outerRadius, outerRadius, outerRadius]} />

                {/* Сфера-граница */}
                <mesh>
                    <sphereGeometry args={[outerRadius, 64, 64]} />
                    <meshPhysicalMaterial
                        transmission={1}
                        roughness={0}
                        thickness={1}
                        transparent
                        opacity={0.3}
                        clearcoat={1}
                        metalness={0}
                        reflectivity={0.5}
                    />
                </mesh>

                {<Suspense fallback={null}>
                    {data.Particles.slice(0, visibleParticles).map((particle, index) => (
                        <ParticleMesh key = { index } particle = { particle } />
                    ))}
                </Suspense>}

                <OrbitControls />
            </Canvas>
        </div>
    );
}

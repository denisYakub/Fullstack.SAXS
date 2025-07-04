import React, { Suspense, useState } from 'react';
import { Canvas } from '@react-three/fiber';
import { OrbitControls } from '@react-three/drei';
import Icosahedron from './Icosahedron';

export default function SysView({ data }) {
    const outerRadius = data.OuterRadius * 2;
    const totalParticles = data.Particles.length;
    const maxSteps = Math.ceil(totalParticles / 10);
    const [visibleCount, setVisibleCount] = useState(maxSteps);

    return (
        <div style={{ width: '100%', height: '100%', position: 'relative' }}>
            {/* Ползунок */}
            <div style={{
                position: 'absolute',
                top: '1rem',
                right: '1rem',
                background: 'rgba(0,0,0,0.5)',
                padding: '0.5rem 1rem',
                borderRadius: '0.5rem',
                zIndex: 10,
                color: 'white',
                fontSize: '0.9rem'
            }}>
                <label>
                    Show: {Math.min(visibleCount * 10, totalParticles)} / {totalParticles}
                    <input
                        type="range"
                        min="1"
                        max={maxSteps}
                        value={visibleCount}
                        onChange={(e) => setVisibleCount(Number(e.target.value))}
                        style={{ width: '150px', marginLeft: '10px' }}
                    />
                </label>
            </div>

            {/* Canvas в 100% */}
            <Canvas
                style={{ position: 'absolute', top: 0, left: 0, width: '100%', height: '100%' }}
                camera={{ position: [0, 0, outerRadius * 1.2], fov: 60 }}
            >
                <ambientLight />
                <pointLight position={[outerRadius, outerRadius, outerRadius]} />

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

                <Suspense fallback={null}>
                    {data.Particles.slice(0, visibleCount * 10).map((particle, index) => (
                        <Icosahedron key={index} particle={particle} />
                    ))}
                </Suspense>

                <OrbitControls />
            </Canvas>
        </div>
    );
}

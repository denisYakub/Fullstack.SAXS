import React, { Suspense } from 'react';
import { Canvas } from '@react-three/fiber';
import { OrbitControls } from '@react-three/drei';
import Icosahedron from './Icosahedron';

export default function SysView({ data }) {
    const outerRadius = data.OuterRadius;

    return (
        <Canvas camera={{ position: [0, 0, outerRadius * 1.2], fov: 60 }}>
            <ambientLight />
            <pointLight position={[outerRadius, outerRadius, outerRadius]} />
            <Suspense fallback={null}>
                {data.Particles.map((particle, index) => (
                    <Icosahedron
                        key={index}
                        vertices={particle.Vertices}
                        center={particle.Center}
                    />
                ))}
            </Suspense>
            <OrbitControls />
        </Canvas>
    );
}
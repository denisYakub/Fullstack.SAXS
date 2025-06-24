import React, { Suspense } from 'react';
import { Canvas } from '@react-three/fiber';
import { OrbitControls } from '@react-three/drei';
import Icosahedron from './Icosahedron';

export default function SysView({ data }) {
    const outerRadius = data.OuterRadius;

    return (<div style={{ top: 0, left: 0, width: '100vw', height: '100vh' }}>
        <Canvas camera={{ position: [0, 0, outerRadius * 1.2], fov: 60 }}>
            <ambientLight />
            <pointLight position={[outerRadius, outerRadius, outerRadius]} />
            <Suspense fallback={null}>
                {data.Particles.map((particle, index) => (
                    <Icosahedron
                        key={index}
                        particle={particle}
                    />
                ))}
            </Suspense>
            <OrbitControls />
        </Canvas>
        </div>
        
    );
}
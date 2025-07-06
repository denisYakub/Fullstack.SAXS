import React from 'react';
import { C60Lines, ICosahedronLines } from './DrawParticle'

export default function ParticleMesh({ particle }) {
    const position = [particle.Center.X, particle.Center.Y, particle.Center.Z];

    const click = (e) => {
        e.stopPropagation();
        console.log(particle);
    };

    let meshElement;
    switch (particle.ParticleType) {
        case 0:
            meshElement = <ICosahedronLines vertices={particle.Vertices} />;
            break;
        case 1:
            meshElement = <C60Lines vertices={particle.Vertices} />;
            break;
        default:
            throw new Error('Unknown particle type');
    }

    return (
        <group position={position} onClick={click}>
            {meshElement}
        </group>
    );
}

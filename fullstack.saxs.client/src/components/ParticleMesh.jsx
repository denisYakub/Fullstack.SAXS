import React from 'react';
import { IcosahedronLines, C60Lines, C70Lines, C240Lines, C540Lines } from './DrawParticle'

export default function ParticleMesh({ particle }) {
    const position = [particle.Center.X, particle.Center.Y, particle.Center.Z];

    const click = (e) => {
        e.stopPropagation();
        console.log(particle);
    };

    let meshElement;
    switch (particle.ParticleType) {
        case 0:
            meshElement = <IcosahedronLines vertices={particle.Vertices} />;
            break;
        case 1:
            meshElement = <C60Lines vertices={particle.Vertices} />;
            break;
        case 2:
            meshElement = <C70Lines vertices={particle.Vertices} />;
            break;
        case 3:
            meshElement = <C240Lines vertices={particle.Vertices} />;
            break;
        case 4:
            meshElement = <C540Lines vertices={particle.Vertices} />;
            break;
        default:
            throw new Error(`Particle type ${particle.ParticleType} is not supported.`);
    }

    return (
        <group position={position} onClick={click}>
            {meshElement}
        </group>
    );
}

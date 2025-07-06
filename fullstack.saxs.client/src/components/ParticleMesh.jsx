import React from 'react';

const ICOSAHEDRON_FACES = [
    [0, 11, 5], [0, 5, 1],
    [0, 1, 7], [0, 7, 10],
    [0, 10, 11], [1, 5, 9],
    [5, 11, 4], [11, 10, 2],
    [10, 7, 6], [7, 1, 8],
    [3, 9, 4], [3, 4, 2],
    [3, 2, 6], [3, 6, 8],
    [3, 8, 9], [4, 9, 5],
    [2, 4, 11], [6, 2, 10],
    [8, 6, 7], [9, 8, 1]
];

const C60_FACES = [
    [0, 3, 8, 5, 1], [2, 7, 15, 13, 6],
    [4, 10, 18, 20, 11], [9, 14, 23, 27, 17],
    [12, 21, 31, 29, 19], [16, 26, 36, 35, 25],
    [22, 32, 42, 43, 33], [24, 30, 40, 44, 34],
    [28, 39, 49, 48, 38], [37, 47, 55, 54, 46],
    [41, 45, 53, 57, 50], [51, 52, 56, 59, 58],
    [0, 1, 4, 11, 7, 2], [0, 2, 6, 14, 9, 3],
    [1, 5, 12, 19, 10, 4], [3, 9, 17, 26, 16, 8],
    [5, 8, 16, 25, 21, 12], [6, 13, 22, 33, 23, 14],
    [7, 11, 20, 30, 24, 15], [10, 19, 29, 39, 28, 18],
    [13, 15, 24, 34, 32, 22], [17, 27, 37, 46, 36, 26],
    [18, 28, 38, 40, 30, 20], [21, 25, 35, 45, 41, 31],
    [23, 33, 43, 47, 37, 27], [29, 31, 41, 50, 49, 39],
    [32, 34, 44, 52, 51, 42], [35, 36, 46, 54, 53, 45],
    [38, 48, 56, 52, 44, 40], [42, 51, 58, 55, 47, 43],
    [48, 49, 50, 57, 59, 56], [53, 54, 55, 58, 59, 57]
];

export default function ParticleMesh({ particle }) {
    const flatVertices = Object.values(particle.Vertices).flatMap(v => [v.X, v.Y, v.Z]);
    const position = [particle.Center.X, particle.Center.Y, particle.Center.Z];

    // Выбор граней по типу частицы
    let faces = [];
    switch (particle.ParticleType) {
        case 0:
            faces = ICOSAHEDRON_FACES;
            break;
        case 1:
            faces = C60_FACES;
            break;
        default:
            throw new Error('Unknown particle type');
    }

    const indices = faces.flat();

    const click = (e) => {
        e.stopPropagation();
        console.log(particle);
    };

    return (
        <group position={position}>
            <mesh onClick={click}>
                <bufferGeometry>
                    <bufferAttribute
                        attach="attributes-position"
                        array={new Float32Array(flatVertices)}
                        count={particle.Vertices.length}
                        itemSize={3}
                    />
                    <bufferAttribute
                        attach="index"
                        array={new Uint16Array(indices)}
                        count={indices.length}
                        itemSize={1}
                    />
                </bufferGeometry>
                <meshStandardMaterial color="#fb923c" wireframe />
            </mesh>
        </group>
    );
}

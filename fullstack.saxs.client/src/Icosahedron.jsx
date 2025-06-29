import React from 'react';

const faces = [
    [0, 11, 5],
    [0, 5, 1],
    [0, 1, 7],
    [0, 7, 10],
    [0, 10, 11],
    [1, 5, 9],
    [5, 11, 4],
    [11, 10, 2],
    [10, 7, 6],
    [7, 1, 8],
    [3, 9, 4],
    [3, 4, 2],
    [3, 2, 6],
    [3, 6, 8],
    [3, 8, 9],
    [4, 9, 5],
    [2, 4, 11],
    [6, 2, 10],
    [8, 6, 7],
    [9, 8, 1]
];

export default function Icosahedron({ particle }) {
    const flatVertices = particle.Vertices.flatMap(v => [v.X, v.Y, v.Z]);
    const position = [particle.Center.X, particle.Center.Y, particle.Center.Z];

    const click = (e) => {
        e.stopPropagation();
        console.log(particle);
        //console.log(JSON.stringify(particle.Vertices.map(v => [v.X, v.Y, v.Z]), null, 2));
        //console.log(JSON.stringify(position, null, 2))
    }

    return (
        <group position={position}>
            <mesh onClick={(e) => click(e)}>
                <bufferGeometry>
                    <bufferAttribute
                        attach="attributes-position"
                        array={new Float32Array(flatVertices)}
                        count={particle.Vertices.length}
                        itemSize={3}
                    />
                    <bufferAttribute
                        attach="index"
                        array={new Uint16Array(faces.flat())}
                        count={faces.length * 3}
                        itemSize={1}
                    />
                </bufferGeometry>
                <meshStandardMaterial color="orange" wireframe />
            </mesh>
        </group>
    );
}

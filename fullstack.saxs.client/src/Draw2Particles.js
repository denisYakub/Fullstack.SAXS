import React from "react";
import { Canvas } from "@react-three/fiber";
import { OrbitControls } from "@react-three/drei";

const vertices1 = [
    [-3, 4.854102, 0], [3, 4.854102, 0], [-3, -4.854102, 0], [3, -4.854102, 0], [0, -3, 4.854102], [0, 3, 4.854102], [0, -3, -4.854102], [0, 3, -4.854102], [4.854102, 0, -3], [4.854102, 0, 3], [-4.854102, 0, -3], [-4.854102, 0, 3]
];

const vertices2 = [
    [3.5, 6.118034, 2], [5.5, 6.118034, 2], [3.5, 2.881966, 2], [5.5, 2.881966, 2], [4.5, 3.5, 3.618034], [4.5, 5.5, 3.618034], [4.5, 3.5, 0.381966], [4.5, 5.5, 0.381966], [6.118034, 4.5, 1], [6.118034, 4.5, 3], [2.881966, 4.5, 1], [2.881966, 4.5, 3]
];

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

function Icosahedron1() {
    return (
        <mesh>
            <bufferGeometry attach="geometry">
                <bufferAttribute
                    attach="attributes-position"
                    count={vertices1.length}
                    array={new Float32Array(vertices1.flat())}
                    itemSize={3}
                />
                <bufferAttribute
                    attach="index"
                    count={faces.length * 3}
                    array={new Uint16Array(faces.flat())}
                    itemSize={1}
                />
            </bufferGeometry>
            <meshStandardMaterial color="orange" wireframe />
        </mesh>
    );
}
function Icosahedron2() {
    return (
        <mesh>
            <bufferGeometry attach="geometry">
                <bufferAttribute
                    attach="attributes-position"
                    count={vertices2.length}
                    array={new Float32Array(vertices2.flat())}
                    itemSize={3}
                />
                <bufferAttribute
                    attach="index"
                    count={faces.length * 3}
                    array={new Uint16Array(faces.flat())}
                    itemSize={1}
                />
            </bufferGeometry>
            <meshStandardMaterial color="orange" wireframe />
        </mesh>
    );
}
export default function App() {
    return (
        <Canvas camera={{ position: [10, 10, 10], fov: 50 }}>
            <ambientLight />
            <directionalLight position={[10, 10, 5]} />
            <Icosahedron1 />
            <Icosahedron2 />
            <OrbitControls />
        </Canvas>
    );
}
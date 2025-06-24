import React from "react";
import { useState } from "react";
import { Canvas } from "@react-three/fiber";
import { OrbitControls } from "@react-three/drei";

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

function Icosahedron({ vertices, center }) {
    const position = [center.X, center.Y, center.Z];

    return (
            <mesh>
                <bufferGeometry attach="geometry">
                    <bufferAttribute
                        attach="attributes-position"
                        count={vertices.length}
                        array={new Float32Array(vertices.flat())}
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
    const [vertices1Text, setVertices1Text] = useState('');
    const [center1Text, setCenter1Text] = useState('');
    const [vertices2Text, setVertices2Text] = useState('');
    const [center2Text, setCenter2Text] = useState('');

    const [vertices1, setVertices1] = useState(null);
    const [vertices2, setVertices2] = useState(null);
    const [center1, setCenter1] = useState(null);
    const [center2, setCenter2] = useState(null);
    const [draw, setDraw] = useState(false);

    const handleDraw = () => {
        try {
            const v1 = JSON.parse(vertices1Text);
            const c1 = JSON.parse(center1Text);
            const v2 = JSON.parse(vertices2Text);
            const c2 = JSON.parse(center2Text);

            if (Array.isArray(v1) && Array.isArray(v2) && c1 && c2) {
                setVertices1(v1);
                setCenter1(c1);
                setVertices2(v2);
                setCenter2(c2);
                setDraw(true);
            }
        } catch {
            alert("Ошибка парсинга JSON: проверь ввод.");
        }
    };

    return (
        <div style={{ top: 0, left: 0, width: '100vw', height: '100vh' }}>
            <div style={{ margin: '1rem' }}>
                <input
                    type="text"
                    placeholder="Vertices 1 (JSON)"
                    value={vertices1Text}
                    onChange={(e) => setVertices1Text(e.target.value)}
                    style={{ width: '45%', marginRight: '1rem' }}
                />
                <input
                    type="text"
                    placeholder="Center 1 (JSON)"
                    value={center1Text}
                    onChange={(e) => setCenter1Text(e.target.value)}
                    style={{ width: '45%' }}
                />
            </div>
            <div style={{ margin: '1rem' }}>
                <input
                    type="text"
                    placeholder="Vertices 2 (JSON)"
                    value={vertices2Text}
                    onChange={(e) => setVertices2Text(e.target.value)}
                    style={{ width: '45%', marginRight: '1rem' }}
                />
                <input
                    type="text"
                    placeholder="Center 2 (JSON)"
                    value={center2Text}
                    onChange={(e) => setCenter2Text(e.target.value)}
                    style={{ width: '45%' }}
                />
            </div>
            <button onClick={handleDraw} style={{ margin: '1rem', padding: '0.5rem 1rem' }}>
                Draw
            </button>
            {draw && (<Canvas camera={{ position: [-2.4270363, 6.009531, 12.535042], fov: 50 }}>
                <ambientLight />
                <directionalLight position={[10, 10, 5]} />
                <Icosahedron vertices={vertices1} center={center1} />
                <Icosahedron vertices={vertices2} center={center2} />
                <OrbitControls />
            </Canvas>)}
        </div>
    );
}
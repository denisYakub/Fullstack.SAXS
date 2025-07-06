import { Canvas } from '@react-three/fiber';
import { OrbitControls } from '@react-three/drei';
import * as THREE from 'three';

const C60_VERTICES = [
    [0, 0, 1.021],
    [0.4035482, 0, 0.9378643],
    [-0.2274644, 0.3333333, 0.9378643],
    [-0.1471226, -0.375774, 0.9378643],
    [0.579632, 0.3333333, 0.7715933],
    [0.5058321, -0.375774, 0.8033483],
    [-0.6020514, 0.2908927, 0.7715933],
    [-0.05138057, 0.6666667, 0.7715933],
    [0.1654988, -0.6080151, 0.8033483],
    [-0.5217096, -0.4182147, 0.7715933],
    [0.8579998, 0.2908927, 0.4708062],
    [0.3521676, 0.6666667, 0.6884578],
    [0.7841999, -0.4182147, 0.5025612],
    [-0.657475, 0.5979962, 0.5025612],
    [-0.749174, -0.08488134, 0.6884578],
    [-0.3171418, 0.8302373, 0.5025612],
    [0.1035333, -0.8826969, 0.5025612],
    [-0.5836751, -0.6928964, 0.4708062],
    [0.8025761, 0.5979962, 0.2017741],
    [0.9602837, -0.08488134, 0.3362902],
    [0.4899547, 0.8302373, 0.3362902],
    [0.7222343, -0.6928964, 0.2017741],
    [-0.8600213, 0.5293258, 0.1503935],
    [-0.9517203, -0.1535518, 0.3362902],
    [-0.1793548, 0.993808, 0.1503935],
    [0.381901, -0.9251375, 0.2017741],
    [-0.2710537, -0.9251375, 0.3362902],
    [-0.8494363, -0.5293258, 0.2017741],
    [0.8494363, 0.5293258, -0.2017741],
    [1.007144, -0.1535518, -0.06725804],
    [0.2241935, 0.993808, 0.06725804],
    [0.8600213, -0.5293258, -0.1503935],
    [-0.7222343, 0.6928964, -0.2017741],
    [-1.007144, 0.1535518, 0.06725804],
    [-0.381901, 0.9251375, -0.2017741],
    [0.1793548, -0.993808, -0.1503935],
    [-0.2241935, -0.993808, -0.06725804],
    [-0.8025761, -0.5979962, -0.2017741],
    [0.5836751, 0.6928964, -0.4708062],
    [0.9517203, 0.1535518, -0.3362902],
    [0.2710537, 0.9251375, -0.3362902],
    [0.657475, -0.5979962, -0.5025612],
    [-0.7841999, 0.4182147, -0.5025612],
    [-0.9602837, 0.08488134, -0.3362902],
    [-0.1035333, 0.8826969, -0.5025612],
    [0.3171418, -0.8302373, -0.5025612],
    [-0.4899547, -0.8302373, -0.3362902],
    [-0.8579998, -0.2908927, -0.4708062],
    [0.5217096, 0.4182147, -0.7715933],
    [0.749174, 0.08488134, -0.6884578],
    [0.6020514, -0.2908927, -0.7715933],
    [-0.5058321, 0.375774, -0.8033483],
    [-0.1654988, 0.6080151, -0.8033483],
    [0.05138057, -0.6666667, -0.7715933],
    [-0.3521676, -0.6666667, -0.6884578],
    [-0.579632, -0.3333333, -0.7715933],
    [0.1471226, 0.375774, -0.9378643],
    [0.2274644, -0.3333333, -0.9378643],
    [-0.4035482, 0, -0.9378643],
    [0, 0, -1.021]
].map(([x, y, z]) => ({ X: x, Y: y, Z: z }));

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

const PHI = (1 + Math.sqrt(5)) / 2;

const Icosahedron_VERTICES = [
    [-1, PHI, 0],
    [1, PHI, 0],
    [-1, -PHI, 0],
    [1, -PHI, 0],
    [0, -1, PHI],
    [0, 1, PHI],
    [0, -1, -PHI],
    [0, 1, -PHI],
    [PHI, 0, -1],
    [PHI, 0, 1],
    [-PHI, 0, -1],
    [-PHI, 0, 1],
].map(([x, y, z]) => ({ X: x, Y: y, Z: z }));;

const Icosahedron_FACES = [
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

export function C60Lines({ vertices }) {
    const flatVertices = vertices.map(v => [v.X, v.Y, v.Z]);
    const points = [];

    for (const face of C60_FACES) {
        for (let i = 0; i < face.length; i++) {
            const from = flatVertices[face[i]];
            const to = flatVertices[face[(i + 1) % face.length]];

            if (from && to) {
                points.push(new THREE.Vector3(...from));
                points.push(new THREE.Vector3(...to));
            }
        }
    }

    const geometry = new THREE.BufferGeometry().setFromPoints(points);

    return (
        <lineSegments geometry={geometry}>
            <lineBasicMaterial color="#ff6900" />
        </lineSegments>
    );
}

export function ICosahedronLines({ vertices }) {
    const flatVertices = vertices.map(v => [v.X, v.Y, v.Z]);
    const points = [];

    for (const face of Icosahedron_FACES) {
        for (let i = 0; i < face.length; i++) {
            const from = flatVertices[face[i]];
            const to = flatVertices[face[(i + 1) % face.length]];

            if (from && to) {
                points.push(new THREE.Vector3(...from));
                points.push(new THREE.Vector3(...to));
            }
        }
    }

    const geometry = new THREE.BufferGeometry().setFromPoints(points);

    return (
        <lineSegments geometry={geometry}>
            <lineBasicMaterial color="#ff6900" />
        </lineSegments>
    );
}

export function DrawParticle() {
    return (
        <div className="min-h-screen flex flex-col bg-gray-300 justify-center items-center text-white px-4 pt-8">
            <div className="max-w mx-auto p-6 bg-gray-600 text-white rounded-md shadow-lg mt-8">
                <h1>C60</h1>
                <Canvas camera={{ position: [0, 0, 4], fov: 50 }}>
                    <ambientLight />
                    <directionalLight position={[3, 3, 3]} />
                    <C60Lines vertices={ C60_VERTICES } />
                    <OrbitControls />
                </Canvas>
            </div>
            <div className="max-w mx-auto p-6 bg-gray-600 text-white rounded-md shadow-lg mt-8">
                <h1>Icosahedron</h1>
                <Canvas camera={{ position: [0, 0, 4], fov: 50 }}>
                    <ambientLight />
                    <directionalLight position={[3, 3, 3]} />
                    <ICosahedronLines vertices={ Icosahedron_VERTICES } />
                    <OrbitControls />
                </Canvas>
            </div>
        </div>
    );
}
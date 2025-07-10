import Vector3 from './Vector3';

export default class Particle {
    constructor(Center, ParticleType, Vertices = []) {
        this.Center = new Vector3(Center.X, Center.Y, Center.Z);
        this.ParticleType = ParticleType;
        this.Vertices = Vertices.map(v => new Vector3(v.X, v.Y, v.Z));
    }

    get particleTypeName() {
        const types = ['Icosahedron', 'C60', 'C70', 'C240', 'C540'];
        return types[this.ParticleType] || 'Unknown';
    }
}
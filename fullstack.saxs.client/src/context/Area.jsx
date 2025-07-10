import Particle from './Particle';

export default class Area {
    constructor(OuterRadius, Particles = []) {
        this.OuterRadius = OuterRadius;
        this.Particles = Particles.map(
            p => new Particle(p.Center, p.ParticleType, p.Vertices)
        );
    }

    getFirstParticleTypeName() {
        return this.Particles.length > 0
            ? this.Particles[0].particleTypeName
            : null;
    }
}
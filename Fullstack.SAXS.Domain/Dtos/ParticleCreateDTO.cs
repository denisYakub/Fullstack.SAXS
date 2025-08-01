﻿using Fullstack.SAXS.Domain.Enums;

namespace Fullstack.SAXS.Domain.Dtos
{
    public record ParticleCreateDTO(
        int Number,
        ParticleTypes Type,
        double MinSize, double MaxSize,
        double SizeShape, double SizeScale,
        double AlphaRotation, double BetaRotation, double GammaRotation,
        double MinX, double MaxX,
        double MinY, double MaxY,
        double MinZ, double MaxZ
    );
}

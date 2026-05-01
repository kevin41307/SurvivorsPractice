using SpatialHash2D;
using NUnit.Framework;
using UnityEngine;

namespace Tests.EditMode
{
    public sealed class SpatialHashGrid2DTests
    {
        [Test]
        public void WorldToCell_ShouldFloorDivide_WhenPositiveCoordinates()
        {
            var grid = new SpatialHashGrid2D(cellSize: 4f);

            var cell = grid.WorldToCell(new Vector2(7.9f, 8.0f));

            Assert.AreEqual(new Vector2Int(1, 2), cell);
        }

        [Test]
        public void IsWithinRadiusSqr_ShouldReturnTrue_WhenDistanceLessThanRadius()
        {
            var radiusSqr = SpatialHashGrid2D.RadiusToSqr(2f);

            var inside = SpatialHashGrid2D.IsWithinRadiusSqr(
                a: new Vector2(0f, 0f),
                b: new Vector2(1f, 1f),
                radiusSqr: radiusSqr);

            Assert.IsTrue(inside);
        }

        [Test]
        public void IsWithinRadiusSqr_ShouldReturnFalse_WhenDistanceGreaterThanOrEqualRadius()
        {
            var radiusSqr = SpatialHashGrid2D.RadiusToSqr(2f);

            var outside = SpatialHashGrid2D.IsWithinRadiusSqr(
                a: new Vector2(0f, 0f),
                b: new Vector2(2f, 0f),
                radiusSqr: radiusSqr);

            Assert.IsFalse(outside);
        }
    }
}

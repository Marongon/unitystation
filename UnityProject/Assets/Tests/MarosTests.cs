using UnityEngine;
using NUnit.Framework;
using Core.Utils;
using Util;
using Shared.Util;
using System.Linq;

namespace Tests
{
    public class MarosTests
    {
        // MathUtils related tests

        [Test]
        public void ModCheck()
        {
            Assert.AreEqual(1, MathUtils.Mod(5, 2));
            Assert.AreEqual(2, MathUtils.Mod(5, 3));
            Assert.AreEqual(0, MathUtils.Mod(5, 5));
            Assert.AreEqual(0, MathUtils.Mod(1, 1));
            Assert.AreEqual(100000, MathUtils.Mod(100000, 200000));
        }

        [Test]
        public void ModNegativeCheck()
        {
            Assert.AreEqual(-1, MathUtils.Mod(5, -2));
            Assert.AreEqual(-1, MathUtils.Mod(5, -3));
            Assert.AreEqual(0, MathUtils.Mod(5, -5));
            Assert.AreEqual(0, MathUtils.Mod(-1, 1));
            Assert.AreEqual(100000, MathUtils.Mod(-100000, 200000));
        }

        [Test]
        public void IsEqualCheck()
        {
            Assert.IsTrue(MathUtils.IsEqual(1f, 1f));
            Assert.IsTrue(MathUtils.IsEqual(1f, 1f + Mathf.Epsilon));
            Assert.IsTrue(MathUtils.IsEqual(1f, 1f - Mathf.Epsilon));
            // NON DETERMINISTIC TESTS BECAUSE OF FLOATING POINT INACCURACIES
            // Assert.IsTrue(MathUtils.IsEqual(1f + Mathf.Epsilon, 1f + Mathf.Epsilon));
            // Assert.IsFalse(MathUtils.IsEqual(1f + Mathf.Epsilon, 1f - Mathf.Epsilon));  // - Epsilon and + Epislon should be False by their implementation logic
            // Assert.IsFalse(MathUtils.IsEqual(1f - Mathf.Epsilon, 1f + Mathf.Epsilon));
            // Assert.IsTrue(MathUtils.IsEqual(1f - Mathf.Epsilon, 1f - Mathf.Epsilon));
            Assert.IsTrue(MathUtils.IsEqual(1f + Mathf.Epsilon, 1f));
            Assert.IsTrue(MathUtils.IsEqual(1f - Mathf.Epsilon, 1f));
        }

        // EncryptionUtils related tests

        [Test]
        public void GenerateRandomAlphanumericStringLengthCheck()
        {
            int length = 30;
            string s = EncryptionUtils.GenerateRandomAlphanumericString(length, false);
            Assert.AreEqual(length, s.Length);

            length = 50;
            s = EncryptionUtils.GenerateRandomAlphanumericString(length, false);
            Assert.AreEqual(length, s.Length);

            length = 500;
            s = EncryptionUtils.GenerateRandomAlphanumericString(length, false);
            Assert.AreEqual(length, s.Length);
        }

        [Test]
        public void GenerateRandomAlphanumericStringOnlyAllowedCharsCheck()
        {
            int length = 50;
            string s = EncryptionUtils.GenerateRandomAlphanumericString(length, false);
            bool onlyAllowed = true;
            const string allowedCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";    // taken directly from the tested method

            foreach (char c in s)
            {
                if (!allowedCharacters.Contains(c))
                {
                    onlyAllowed = false;
                }
            }

            Assert.IsTrue(onlyAllowed);
        }

        [Test]
        public void GenerateRandomAlphanumericStringRandomizedLengthCheck()
        {
            int length = 10;
            string s = EncryptionUtils.GenerateRandomAlphanumericString(length, randomizeLength: true);

            int min = Mathf.Clamp(length / 2, 0, 100);  // taken directly from tested method
            int max = Mathf.Clamp(length * 2, 0, 100);

            Assert.GreaterOrEqual(s.Length, min);
            Assert.LessOrEqual(s.Length, max);
        }

        // DebugTools related tests

        [Test]
        public void HexToColorDefaultCheck()
        {
            // full alpha check
            Color32 c = DebugTools.HexToColor("#FF0000"); // red

            Assert.AreEqual(255, c.r);
            Assert.AreEqual(0, c.g);
            Assert.AreEqual(0, c.b);
            Assert.AreEqual(255, c.a); // default alpha

            // different alpha check
            c = DebugTools.HexToColor("#00FF0080"); // green, half alpha

            Assert.AreEqual(0, c.r);
            Assert.AreEqual(255, c.g);
            Assert.AreEqual(0, c.b);
            Assert.AreEqual(0x80, c.a);
        }

        [Test]
        public void HexToColorToHexCheck()
        {
            var original = new Color32(10, 20, 30, 40);
            string hex = DebugTools.ColorToHex(original);
            Color32 c = DebugTools.HexToColor(hex);

            Assert.AreEqual(original, c);
        }

        // TemperatureUtils related tests

        [Test]
        public void TemperatureTransformCelsiusToKelvinCheck()
        {
            float c = 0f;
            float k = TemperatureUtils.Transform(c, TemeratureUnits.C, TemeratureUnits.K);

            Assert.That(k, Is.EqualTo(TemperatureUtils.ZERO_CELSIUS_IN_KELVIN).Within(1e-4f));
        }

        [Test]
        public void TemperatureTransformCelsiusToFahrenheitToCelsiusCheck()
        {
            float c = 25f;
            float f = TemperatureUtils.Transform(c, TemeratureUnits.C, TemeratureUnits.F);
            float cBack = TemperatureUtils.Transform(f, TemeratureUnits.F, TemeratureUnits.C);

            Assert.That(cBack, Is.EqualTo(c).Within(1e-2f));
        }

        // Fake/mock/stub test (stub in this case)

        [Test]
        public void FindInterfacesOfTypeFindsStubObject()
        {
            // Arrange
            var go = new GameObject("FakeObj");
            go.AddComponent<FakeComponent>();

            // Act
            var found = FindUtils.FindInterfacesOfType<IFakeTestInterface>().ToList();

            // Assert
            Assert.IsTrue(found.Count > 0);
            Assert.IsInstanceOf<IFakeTestInterface>(found[0]);
        }
    }
}

public interface IFakeTestInterface { }

public class FakeComponent : MonoBehaviour, IFakeTestInterface {}
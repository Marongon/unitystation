using System.Diagnostics;
using NUnit.Framework;
using Systems.Spawns;
using Systems.Teleport;
using UnityEngine;

namespace Tests
{
    public class MarosProfilingTests
    {
        [Test]
        public void GetSpawnDestinationsBeforeProfiling()
        {
            const int spawnCount = 200;
            for (int i = 0; i < spawnCount; i++)
            {
                new GameObject("SpawnPoint_" + i).AddComponent<SpawnPoint>();
            }

            const int iterations = 1000;
            var sw = new Stopwatch();
            sw.Start();

            int count = 0;
            for (int i = 0; i < iterations; i++)
            {
                foreach (var dest in TeleportUtils.GetSpawnDestinations())
                {
                    count++;
                }
            }

            sw.Stop();
            UnityEngine.Debug.Log($"OLD: {iterations} calls, {spawnCount} spawnpoints, time = {sw.ElapsedMilliseconds} ms");
        }

        [Test]
        public void GetSpawnDestinationsAfterProfiling()
        {
            const int spawnCount = 200;
            for (int i = 0; i < spawnCount; i++)
            {
                new GameObject("SpawnPoint_" + i).AddComponent<SpawnPoint>();
            }

            const int iterations = 1000;
            var sw = new Stopwatch();
            sw.Start();

            int count = 0;
            for (int i = 0; i < iterations; i++)
            {
                foreach (var dest in TeleportUtils.GetSpawnDestinationsCached())
                {
                    count++;
                }
            }

            sw.Stop();
            UnityEngine.Debug.Log($"CACHED: {iterations} calls, {spawnCount} spawnpoints, time = {sw.ElapsedMilliseconds} ms");
        }
    }
}
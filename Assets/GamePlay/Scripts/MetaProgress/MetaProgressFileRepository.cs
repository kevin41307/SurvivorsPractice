using System;
using System.IO;
using UnityEngine;

namespace GamePlay.Scripts.MetaProgress
{
    public sealed class MetaProgressFileRepository
    {
        public string GetSlotPath(int slotId)
        {
            if (slotId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(slotId), "slotId 必須 >= 1");
            }

            var fileName = $"meta_progress_slot_{slotId}.json";
            return Path.Combine(UnityEngine.Application.persistentDataPath, fileName);
        }

        public MetaProgressData LoadOrCreate(int slotId)
        {
            var path = GetSlotPath(slotId);
            if (!File.Exists(path))
            {
                return MetaProgressData.Create();
            }

            try
            {
                var json = File.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(json))
                {
                    return MetaProgressData.Create();
                }

                var dto = JsonUtility.FromJson<MetaProgressDataDto>(json);
                if (dto == null)
                {
                    return MetaProgressData.Create();
                }

                return MetaProgressData.FromDto(dto);
            }
            catch (Exception)
            {
                // 壞檔：保留原檔，避免直接覆蓋
                TryBackupCorruptedFile(path);
                return MetaProgressData.Create();
            }
        }

        public void Save(int slotId, MetaProgressData progress)
        {
            var path = GetSlotPath(slotId);
            var dto = progress.ToDto();

            var json = JsonUtility.ToJson(dto, prettyPrint: true);
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            File.WriteAllText(path, json);
        }

        private static void TryBackupCorruptedFile(string path)
        {
            try
            {
                var dir = Path.GetDirectoryName(path);
                var name = Path.GetFileNameWithoutExtension(path);
                var ext = Path.GetExtension(path);
                var stamp = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
                var backupPath = Path.Combine(dir ?? "", $"{name}.broken_{stamp}{ext}");
                File.Copy(path, backupPath, overwrite: false);
            }
            catch
            {
                // ignore
            }
        }
    }
}


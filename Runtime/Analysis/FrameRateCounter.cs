using System;
using System.Text;
using TMPro;
using Unity.Profiling;
using UnityEditor;
using UnityEngine;

namespace OT.Extensions.Analysis
{
    public class FrameRateCounter : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI display;

        private StringBuilder _sb;
        private int _mRenderCount = 0;
        private DateTime _mRenderTimer = DateTime.MinValue;
        private int _frames;
        private string _statsText;
        private ProfilerRecorder _totalReservedMemoryRecorder;
        private ProfilerRecorder _gcReservedMemoryRecorder;
        private ProfilerRecorder _systemUsedMemoryRecorder;

        private string _version;


        private void OnEnable()
        {
            _version = PlayerSettings.bundleVersion;
            _totalReservedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Total Reserved Memory");
            _gcReservedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Reserved Memory");
            _systemUsedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "System Used Memory");
        }

        private void OnDisable()
        {
            _totalReservedMemoryRecorder.Dispose();
            _gcReservedMemoryRecorder.Dispose();
            _systemUsedMemoryRecorder.Dispose();
        }

        private void FixedUpdate()
        {
            ++_mRenderCount;

            if (_mRenderTimer < DateTime.Now)
            {
                _mRenderTimer = DateTime.Now + TimeSpan.FromSeconds(1);
                display.text = $"v.[{_version}] - {GetSysInfo(_mRenderCount)}";
                _mRenderCount = 0;
            }
        }


        private string GetSysInfo(int fps)
        {
            _sb = new(200);
            // sb.AppendLine($"{fps} v.{_version}");
            if (_totalReservedMemoryRecorder.Valid)
                _sb.AppendLine(
                    $"FPS: [{fps}] [RAM: {ToSize(_totalReservedMemoryRecorder.LastValue, SizeUnits.GB)}{SizeUnits.GB.ToString()}]");

            /*if (_gcReservedMemoryRecorder.Valid)
                _sb.AppendLine($"GC Reserved Memory: {ToSize(_gcReservedMemoryRecorder.LastValue, SizeUnits.GB)}{SizeUnits.GB.ToString()}");
            if (_systemUsedMemoryRecorder.Valid)
                _sb.AppendLine($"System Used Memory: {ToSize(_systemUsedMemoryRecorder.LastValue, SizeUnits.GB)}{SizeUnits.GB.ToString()}");*/

            return _sb.ToString();
        }

        private static string ToSize(long value, SizeUnits unit)
        {
            return (value / (double)Math.Pow(1024, (long)unit)).ToString("0.00");
        }

        private enum SizeUnits
        {
            Byte,
            KB,
            MB,
            GB,
            TB,
            PB,
            EB,
            ZB,
            YB
        }
    }
}

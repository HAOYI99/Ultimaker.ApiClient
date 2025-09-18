namespace Ultimaker.ApiClient.Core.Constants;

public static class UltimakerPaths
{
    public static class Printer
    {
        public const string Base = "api/v1/printer";
        public const string Status = Base + "/status";
        public const string Led = Base + "/led";
        public const string LedHue = Led + "/hue";
        public const string LedSaturation = Led + "/saturation";
        public const string LedBrightness = Led + "/brightness";
        public const string LedBlink = Led + "/blink";
        public const string Bed = Base + "/bed";
        public const string BedTemperature = Bed + "/temperature";
        public const string BedPreHeat = Bed + "/pre_heat";
        public const string BedType = Bed + "/type";
        public const string Heads = Base + "/heads";
        public static string Head(int headIndex) => $"{Heads}/{headIndex}";
        public static string HeadPosition(int headIndex) => $"{Head(headIndex)}/position";
        public static string HeadMaxSpeed(int headIndex) => $"{Head(headIndex)}/max_speed";
        public static string HeadAcceleration(int headIndex) => $"{Head(headIndex)}/acceleration";
        public static string HeadJerk(int headIndex) => $"{Head(headIndex)}/jerk";
        public static string Extruders(int headIndex) => $"{Head(headIndex)}/extruders";
        public static string Extruder(int headIndex, int extruderIndex) => $"{Extruders(headIndex)}/{extruderIndex}";
        public static string Hotend(int headIndex, int extruderIndex) => $"{Extruder(headIndex, extruderIndex)}/hotend";

        public static string HotendTemperature(int headIndex, int extruderIndex) =>
            $"{Hotend(headIndex, extruderIndex)}/temperature";

        public static string HotendOffset(int headIndex, int extruderIndex) =>
            $"{Hotend(headIndex, extruderIndex)}/offset";

        public static string Feeder(int headIndex, int extruderIndex) => $"{Extruder(headIndex, extruderIndex)}/feeder";
        public static string FeederJerk(int headIndex, int extruderIndex) => $"{Feeder(headIndex, extruderIndex)}/jerk";

        public static string FeederMaxSpeed(int headIndex, int extruderIndex) =>
            $"{Feeder(headIndex, extruderIndex)}/max_speed";

        public static string FeederAcceleration(int headIndex, int extruderIndex) =>
            $"{Feeder(headIndex, extruderIndex)}/acceleration";

        public static string ActiveMaterial(int headIndex, int extruderIndex) =>
            $"{Extruder(headIndex, extruderIndex)}/active_material";

        public static string ActiveMaterialId(int headIndex, int extruderIndex) =>
            $"{ActiveMaterial(headIndex, extruderIndex)}/guid";


        public static string PrinterPath(string path) => $"{Base}/{path}";
    }

    public static class PrintJob
    {
        public const string Base = "api/v1/print_job";
        public const string JobName = Base + "/name";
        public const string PauseSrc = Base + "/pause_source";
        public const string Progress = Base + "/progress";
        public const string ReprintOriginalUuid = Base + "/reprint_original_uuid";
        public const string Result = Base + "/result";
        public const string State = Base + "/state";
        public const string TimeElapsed = Base + "/time_elapsed";
        public const string TimeTotal = Base + "/time_total";
        public const string JobId = Base + "/uuid";
        public const string StartedDt = Base + "/datetime_started";
        public const string FinishedDt = Base + "/datetime_finished";
        public const string CleanedDt = Base + "/datetime_cleaned";
        public const string Source = Base + "/source";
        public const string SourceUser = Base + "/source_user";
        public const string SourceApp = Base + "/source_application";
        public const string GCode = Base + "/gcode";
        public const string Container = Base + "/container";

        public static string PrintJobPath(string path) => $"{Base}/{path}";
    }

    public static class Material
    {
        public const string Base = "api/v1/materials";

        public static string IdPath(Guid id) => $"{Base}/{id}";
        public static string MaterialPath(string path) => $"{Base}/{path}";
    }

    public static class Auth
    {
        public const string Base = "api/v1/auth";
        public const string Request = Base + "/request";
        public const string Verify = Base + "/verify";
        public static string Check(string id) => $"{Base}/check/{id}";

        public static string AuthPath(string path) => $"{Base}/{path}";
    }

    public static class System
    {
        public const string Base = "api/v1/system";
        public const string Platform = Base + "/platform";
        public const string Hostname = Base + "/hostname";
        public const string Firmware = Base + "/firmware";
        public const string FirmwareStatus = Firmware + "/status";
        public const string FirmwareStable = Firmware + "/stable";
        public const string FirmwareLatest = Firmware + "/latest";
        public const string Memory = Base + "/memory";
        public const string Time = Base + "/time";
        public const string Logs = Base + "/log";
        public const string Name = Base + "/name";
        public const string Country = Base + "/country";
        public const string IsCountryLocked = Base + "/is_country_locked";
        public const string Language = Base + "/language";
        public const string Uptime = Base + "/uptime";
        public const string Type = Base + "/type";
        public const string Variant = Base + "/variant";
        public const string Hardware = Base + "/hardware";
        public const string HardwareTypeId = Hardware + "/typeid";
        public const string HardwareRevision = Hardware + "/revision";
        public const string Guid = Base + "/guid";
        public const string DisplayMessage = Base + "/display_message";

        public static string LogQueryPath(int? boot, int? lines)
        {
            var query = new List<string>();
            if (boot.HasValue) query.Add($"boot={boot.Value}");
            if (lines.HasValue) query.Add($"lines={lines.Value}");
            return query.Count > 0
                ? $"{Logs}?{string.Join("&", query)}"
                : Logs;
        }
        
        public static string SystemPath(string path) => $"{Base}/{path}";
    }

    public static class History
    {
        public const string Base = "api/v1/history";
        public const string PrintJobs = Base + "/print_jobs";
        public const string Events = Base + "/events";

        public static string PrintJobQueryPath(int? offset, int? count)
        {
            var query = new List<string>();
            if (offset.HasValue) query.Add($"offset={offset.Value}");
            if (count.HasValue) query.Add($"count={count.Value}");
            return query.Count > 0
                ? $"{PrintJobs}?{string.Join("&", query)}"
                : PrintJobs;
        }
        
        public static string EventsQueryPath(int? offset, int? count, int? typeId)
        {
            var query = new List<string>();
            if (offset.HasValue) query.Add($"offset={offset.Value}");
            if (count.HasValue) query.Add($"count={count.Value}");
            if (typeId.HasValue) query.Add($"type_id={typeId.Value}");
            return query.Count > 0
                ? $"{Events}?{string.Join("&", query)}"
                : Events;
        }

        public static string PrintJobIdPath(Guid id) => $"{PrintJobs}/{id}";

        public static string HistoryPath(string path) => $"{Base}/{path}";
    }

    public static class AirManager
    {
        public const string Base = "api/v1/airmanager";
    }
}
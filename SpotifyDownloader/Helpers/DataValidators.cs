using System;
using System.Text.RegularExpressions;
using Avalonia.Data;
using SpotifyDownloader.Models;

namespace SpotifyDownloader.Helpers;

public static class DataValidators
{
        // a regex for a spotify playlist or song link
        private static readonly string SearchRegex = @"^https:\/\/open\.spotify\.com\/(playlist|track)\/[a-zA-Z0-9]{22}(\?si=[a-zA-Z0-9_-]+)?$";

        // extension methods
        // for string validation
        public static bool IsValid(this string value, DataTypes dataType)
        {
            return dataType switch
            {
                DataTypes.SearchInput => Regex.IsMatch(value, SearchRegex) ? true : throw new DataValidationException("Invalid playlist or song link"),
                DataTypes.Password => value.Length > 8 ? true : throw new DataValidationException("The password is too short"),
                _ => false,
            };
        }

        // for double validation
        public static bool IsValid(this double? value, DataTypes dataType)
        {
            return dataType switch
            {
                DataTypes.InternetSpeed => value > 1 ? true : throw new DataValidationException("Internet Speed is too slow"),
                _ => false,
            };
        }

        // for date validation
        public static bool IsValid(this DateTimeOffset? value, DataTypes dataType)
        {
            return dataType switch
            {
                DataTypes.InternetSpeed => value?.Year > 1 && value?.Month > 1 && value?.Day > 1 ? true : throw new DataValidationException("Wrong date"),
                _ => false,
            };
        }
}
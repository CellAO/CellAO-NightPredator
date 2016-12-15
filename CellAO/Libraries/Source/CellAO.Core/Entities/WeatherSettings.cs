#region License

// Copyright (c) 2005-2016, CellAO Team
// 
// 
// All rights reserved.
// 
// 
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// 
// 
//     * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
//     * Neither the name of the CellAO Team nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
// 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
// A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
// EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
// PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
// PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
// LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
// NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// 

#endregion

namespace CellAO.Core.Entities
{
    #region Usings ...

    using System;
    using System.Collections.Generic;
    using System.Linq;

    #endregion

    public class WeatherSettings
    {
        private static WeatherSettings instance = null;

        private readonly List<WeatherEntry> weatherList = new List<WeatherEntry>();

        public List<WeatherEntry> WeatherList
        {
            get
            {
                // Cleanup of old entries before returning the list
                var temp =
                    this.weatherList.Where(
                        x => (x.FadeIn+x.FadeOut) / 100 * 60 + x.Duration < (DateTime.UtcNow - x.StartTime).TotalSeconds);
                foreach (var t in temp)
                {
                    this.weatherList.Remove(t);
                }
                return this.weatherList;
            }
        }

        public WeatherSettings()
        {
        }

        public static WeatherSettings Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new WeatherSettings();
                }
                return instance;
            }
        }

        public void Add(WeatherEntry entry)
        {
            WeatherEntry alreadyWeatherEntry =
                this.WeatherList.FirstOrDefault(
                    x =>
                        (x.Playfield.Instance == entry.Playfield.Instance) && (x.Playfield.Type == entry.Playfield.Type));
            if (alreadyWeatherEntry != null)
            {
                this.WeatherList.Remove(alreadyWeatherEntry);
            }
            entry.StartTime = DateTime.UtcNow;
            this.WeatherList.Add(entry);
        }
    }
}
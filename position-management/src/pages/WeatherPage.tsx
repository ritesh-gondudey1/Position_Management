import React, { useEffect, useState } from 'react';

type WeatherInfo = {
  temperature: number;
  condition: string;
  location: string;
  date: string;
};

const WeatherPage: React.FC = () => {
  const [weather, setWeather] = useState<WeatherInfo | null>(null);

  useEffect(() => {
      fetch('https://localhost:7075/api/Weather')
      .then(res => res.json())
      .then(data => setWeather(data));
  }, []);

  return (
    <div>
      <h2>Weather Information</h2>
      {weather ? (
        <div>
          <p>Location: {weather.location}</p>
          <p>Temperature: {weather.temperature}°C</p>
          <p>Condition: {weather.condition}</p>
          <p>Date: {new Date(weather.date).toLocaleString()}</p>
        </div>
      ) : (
        <p>Loading...</p>
      )}
    </div>
  );
};

export default WeatherPage;
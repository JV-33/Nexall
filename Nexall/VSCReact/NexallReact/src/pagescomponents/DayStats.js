import Chart from 'chart.js/auto';
import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';

export default function DayStats() {
  const { date } = useParams();
  const [selectedDate, setSelectedDate] = useState(date);
  const [data, setData] = useState([]);
  const [chartData, setChartData] = useState({});
  const [chartInstance, setChartInstance] = useState(null); // Pievienojam chartInstance stāvokli
  const [isLoading, setIsLoading] = useState(false); // Jauns stāvoklis ielādes kontrolei

  useEffect(() => {
    setIsLoading(true); // Sāk ielādi
    fetch(`https://localhost:7241/CarStatistics/DayStats/${selectedDate}`)
      .then(response => {
        if (!response.ok) {
          throw new Error('Network response was not ok');
        }
        return response.json();
      })
      .then(data => {
        setData(data);
        console.log(data);
        setIsLoading(false); // Beidz ielādi
      })
      .catch(error => {
        console.log('Fetch error:', error);
        setIsLoading(false); // Beidz ielādi arī kļūdas gadījumā
      });
  }, [selectedDate]);

  useEffect(() => {
    if (data.length) {
      const speeds = Array(24).fill(0);
      const counts = Array(24).fill(0);

      data.forEach(item => {
        const hour = new Date(item.date).getHours();
        speeds[hour] += item.speed;
        counts[hour]++;
      });

      const averages = speeds.map((sum, index) =>
        counts[index] ? sum / counts[index] : 0
      );

      setChartData({
        labels: Array.from({ length: 24 }, (_, i) => `${i}:00`),
        datasets: [
          {
            label: 'Vidējais ātrums',
            data: averages,
            borderColor: 'rgba(75, 192, 192, 1)',
            fill: false,
          },
        ],
      });
    }
  }, [data]);

  useEffect(() => {
    if (chartData.labels && chartData.datasets) {
      if (chartInstance) {
        chartInstance.destroy(); // Iznīcinām esošo diagrammu, ja tāda ir
      }
      
      const newChartInstance = new Chart(document.getElementById('dayChart'), {
        type: 'line',
        data: chartData,
        options: {
          scales: {
            y: {
              beginAtZero: true,
            },
          },
        },
      });

      setChartInstance(newChartInstance); // Saglabājam jauno diagrammas instanci
    }
  }, [chartData]);

  const handleDateChange = (e) => {
    setSelectedDate(e.target.value);
  };

  return (
    <div>
      <h2>Dienas griezumā</h2>
      <label>
        Izvēlēties datumu:
        <input
          type="date"
          value={selectedDate}
          onChange={handleDateChange}
        />
      </label>
      <button onClick={() => {}}>Apstiprināt</button>
      {isLoading ? (
        <p>Ielāde...</p>
      ) : (
        <canvas id="dayChart" width="400" height="200"></canvas>
      )}
    </div>
  );
}
import axios from 'axios';
import React, { useEffect, useState } from 'react';

function CarStatistics() {
  const [data, setData] = useState([]);

  useEffect(() => {
    async function fetchData() {
      try {
        const response = await axios.get('https://localhost:7241/CarStatistics/');
        setData(response.data);
      } catch (error) {
        console.error("Kļūda, saņemot datus:", error);
      }
    }

    fetchData();
  }, []);

  return (
    <div>
      <h2></h2>
      <table>
        <thead>
          <tr>
            <th>Datums</th>
            <th>Ātrums</th>
            <th>Reģistrācijas numurs</th>
          </tr>
        </thead>
        <tbody>
          {data.map(item => (
            <tr key={item.id}>
              <td>{item.date}</td>
              <td>{item.speed}</td>
              <td>{item.registrationNumber}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );

}

export default CarStatistics;
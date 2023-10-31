import axios from 'axios';
import React, { useEffect, useState } from 'react';

function CarStatistics() {
  const [data, setData] = useState([]);
  const [loading, setLoading] = useState(false);
  const [currentPage, setCurrentPage] = useState(1);
  const pageSize = 70000;

  useEffect(() => {
    async function fetchData() {
      try {
        setLoading(true);
        const response = await axios.get(`https://localhost:7241/CarStatistics/?pageSize=${pageSize}&currentPage=${currentPage}`);
        setData(response.data);
      } catch (error) {
        console.error("Kļūda, saņemot datus:", error);
      } finally {
        setLoading(false);
      }
    }

    fetchData();
  }, [currentPage]);

  return (
    <div>
      {loading ? (
        <div>Loading...</div>
      ) : (
        <div>
          <h2>Auto statistika</h2>
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
          <button onClick={() => setCurrentPage(prev => Math.max(prev - 1, 1))}>Iepriekšējā lappuse</button>
          <button onClick={() => setCurrentPage(prev => prev + 1)}>Nākamā lappuse</button>
        </div>
      )}
    </div>
  );
}

export default CarStatistics;
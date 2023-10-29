import React, { useEffect, useState } from 'react';

export default function Filter() {
  const [data, setData] = useState([]);
  const [filteredData, setFilteredData] = useState([]);
  const [speedFilter, setSpeedFilter] = useState('');
  const [fromDateFilter, setFromDateFilter] = useState('');
  const [toDateFilter, setToDateFilter] = useState('');

  useEffect(() => {
    fetch('https://localhost:7241/CarStatistics/')
      .then(response => response.json())
      .then(data => {
        setData(data);
        setFilteredData(data.slice(0, 20)); // Ņemam tikai pirmās 20 rindas no sākotnējiem datiem
      })
      .catch(error => console.log(error));
  }, []);

  const handleFilter = () => {
    let filtered = data;

    if (speedFilter) {
      filtered = filtered.filter(item => item.speed = speedFilter);
    }

    if (fromDateFilter) {
      filtered = filtered.filter(item => new Date(item.date) >= new Date(fromDateFilter));
    }

    if (toDateFilter) {
      filtered = filtered.filter(item => new Date(item.date) <= new Date(toDateFilter));
    }

    setFilteredData(filtered.slice(0, 20));  // Ņemam tikai pirmās 20 rindas
  };

  return (
    <div>
      <h2>Filtrs</h2>
      <div>
        <label>
          Ātrums:
          <input
            type="number"
            value={speedFilter}
            onChange={e => setSpeedFilter(e.target.value)}
          />
        </label>
        <label>
          Datums no:
          <input
            type="date"
            value={fromDateFilter}
            onChange={e => setFromDateFilter(e.target.value)}
          />
        </label>
        <label>
          Datums līdz:
          <input
            type="date"
            value={toDateFilter}
            onChange={e => setToDateFilter(e.target.value)}
          />
        </label>
        <button onClick={handleFilter}>Filtrēt</button>
      </div>
      <table>
        <thead>
          <tr>
            <th>Datums</th>
            <th>Ātrums</th>
            <th>Reģistrācijas numurs</th>
          </tr>
        </thead>
        <tbody>
          {filteredData.map(item => (
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
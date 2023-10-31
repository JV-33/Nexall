import axios from 'axios';
import React, { useEffect, useState } from 'react';

function CarStatistics() {
  const [data, setData] = useState([]);
  const [loading, setLoading] = useState(false);
  const [filter, setFilter] = useState({ 
    speed: '', 
    startDate: '', 
    endDate: '',
    registrationNumber: ''  // <-- Pievienots reģistrācijas numurs
  });

  const fetchData = async (filterParams = {}) => {
    setLoading(true);
    try {
      const response = await axios.get(`https://localhost:7241/CarStatistics/filtered`, {
        params: {
          speed: filterParams.speed,
          startDate: filterParams.startDate,
          endDate: filterParams.endDate,
          registrationNumber: filterParams.registrationNumber 
        }
      });

      setData(response.data);
    } catch (error) {
      console.error("Kļūda, saņemot datus:", error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchData();
  }, []);

  const handleFilterChange = (event) => {
    const updatedFilter = {
      ...filter,
      [event.target.name]: event.target.value
    };
    console.log("Filtera izmaiņas:", updatedFilter);
    setFilter(updatedFilter);
  };
  

  const handleFilterSubmit = () => {
    console.log("Filtrēt poga nospiesta ar filtriem:", filter);
    fetchData(filter);
  };
  

  const clearFilter = () => {
    const defaultFilter = { speed: '', startDate: '', endDate: '', registrationNumber: '' };
    console.log("Notīrīts filtrs:", defaultFilter);
    setFilter(defaultFilter);  
    fetchData();
  };
  

  return (
    <div>
      <h2>Filtrēšanas iespējas</h2>
      <div>
        <label>Ātrums: </label>
        <input type="number" name="speed" value={filter.speed} onChange={handleFilterChange} />

        <label>Datums no: </label>
        <input type="date" name="startDate" value={filter.startDate} onChange={handleFilterChange} />

        <label>Datums līdz: </label>
        <input type="date" name="endDate" value={filter.endDate} onChange={handleFilterChange} />

        <label>Reģistrācijas numurs: </label>
        <input type="text" name="registrationNumber" value={filter.registrationNumber} onChange={handleFilterChange} /> 

        <button onClick={handleFilterSubmit}>Filtrēt</button>
        <button onClick={clearFilter}>Notīrīt filtru</button>
      </div>

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
        </div>
      )}
    </div>
  );
}

export default CarStatistics;
import React from 'react';
import {
  Route,
  RouterProvider,
  createBrowserRouter,
  createRoutesFromElements
} from 'react-router-dom';
import './App.css';

import DayStats from './pagescomponents/DayStats';
import Filter from './pagescomponents/filter';
import Home from './pagescomponents/home';
import Saraksts from './pagescomponents/saraksts';

import RootLayout from './layouts/RootLayout';

const router = createBrowserRouter(
  createRoutesFromElements( 
  <Route path="/" element={<RootLayout/>}>
    <Route path='/' element={<Home/>}/>
    <Route path='saraksts' element={<Saraksts/>}/>
    <Route path='filter' element={<Filter/>}/>
    <Route path='daystats/:date?' element={<DayStats />}/>
  </Route>
    ))

function App() {
  return (
    <div className="App">
    <RouterProvider router={router}/>
    </div>
  );
}

export default App;
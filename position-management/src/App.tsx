import { useState } from 'react'
import './App.css'
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import WeatherPage from './pages/WeatherPage';
import TransactionPage from './pages/TransactionPage';
import PositionPage from './pages/PositionPage';

function App() {
    return (
        <BrowserRouter>
            <Routes>
                {/* Other routes */}
                <Route path="/weather" element={<WeatherPage />} />
                <Route path="/transactions" element={<TransactionPage />} />
                <Route path="/positions" element={<PositionPage />} />
            </Routes>
        </BrowserRouter>
    );
}

export default App;

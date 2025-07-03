import React from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import HomePage from '../pages/HomePage';
import CreateSystemPage from '../pages/CreateSystemPage';
import AllSystemsPage from '../pages/AllSystemsPage';
import MySystemsPage from '../pages/MySystemsPage';
import SystemDetailsPage from '../pages/SystemDetailsPage';
import Header from '../components/Header'

export default function AppRouter() {
    return (<BrowserRouter>
        <Header />

        <Routes>
            <Route path="/" element={ <HomePage /> } />
            <Route path="/create" element={ <CreateSystemPage /> } />
            <Route path="/all" element={ <AllSystemsPage /> } />
            <Route path="/my" element={ <MySystemsPage /> } />
            <Route path="/system/:id" element={ <SystemDetailsPage /> } />
        </Routes>
    </BrowserRouter>);
}
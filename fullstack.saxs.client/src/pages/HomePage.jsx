import React, { useEffect, useState } from 'react';
import { pingAuth } from '../api/SystemApi';
import LoadingPage from './LoadingPage';

export default function HomePage() {
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        pingAuth()
            .then(
                () => setLoading(false)
            )
            .catch(
                () => window.location.href = '/service-unavailable'
            );
    }, []);

    if (loading) return <LoadingPage />;

    return (
        <div className="min-h-screen flex flex-col bg-gray-300 justify-center items-center text-white px-4 pt-8">
            <div className="max-w-3xl mx-auto p-6 bg-gray-600 text-white rounded-md shadow-lg mt-8">
                <h2 className="text-4xl font-bold mb-6 select-none">Welcome!</h2>
                <p className="text-lg max-w-xl text-center">
                    This is your dashboard. Use the navigation menu to create and manage your systems.
                </p>
            </div>
        </div>
    );
}
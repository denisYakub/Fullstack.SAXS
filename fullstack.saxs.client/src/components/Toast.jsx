import React, { useEffect, useState } from 'react';

export default function Toast({ message, type = 'info', duration = 10000 }) {
    const [visible, setVisible] = useState(true);

    useEffect(() => {
        const timer = setTimeout(() => setVisible(false), duration);
        return () => clearTimeout(timer);
    }, [duration]);

    if (!visible) return null;

    const colors = {
        success: 'bg-green-600',
        warning: 'bg-yellow-500 text-black',
        error: 'bg-red-600',
        info: 'bg-gray-600',
    };

    const background = colors[type] || colors.info;

    return (
        <div className="fixed top-20 right-3 z-50">
            <div className={`${background} px-6 py-4 rounded-lg shadow-lg relative flex items-center space-x-4`}>
                <span className="flex-1">{message}</span>
                <button
                    className="absolute top-1 right-2 text-xl font-bold hover:text-white/80"
                    onClick={() => setVisible(false)}
                >
                    &times;
                </button>
            </div>
        </div>
    );
}

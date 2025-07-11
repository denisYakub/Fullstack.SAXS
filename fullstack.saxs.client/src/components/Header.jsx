import { Link } from 'react-router-dom';

export default function Header() {
    return (
        <header className="flex items-center justify-between bg-gray-600 text-white px-6 py-4 shadow-md">
            <h1 className="text-3xl font-bold text-orange-500 select-none">🌐SAXS</h1>
            <nav className="space-x-6">
                <Link
                    to="/"
                    className="text-lg font-medium hover:text-indigo-400 transition-colors"
                >
                    Home
                </Link>
                <Link
                    to="/create"
                    className="text-lg font-medium hover:text-indigo-400 transition-colors"
                >
                    Create
                </Link>
                <Link
                    to="/all"
                    className="text-lg font-medium hover:text-indigo-400 transition-colors"
                >
                    All
                </Link>
                <Link
                    to="/my"
                    className="text-lg font-medium hover:text-indigo-400 transition-colors"
                >
                    Mine
                </Link>
                <Link
                    to="https://localhost:7135/Identity/Account/Login"
                    className="text-lg font-medium hover:text-indigo-400 transition-colors"
                >
                    Account
                </Link>
            </nav>
        </header>
    );
}

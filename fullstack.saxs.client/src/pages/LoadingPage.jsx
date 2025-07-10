export default function LoadingPage() {
    return (
        <div className="flex justify-center items-center min-h-screen bg-gradient-to-r from-gray-700 via-gray-800 to-gray-900 text-white">
            <div className="text-center space-y-4">
                <div className="animate-spin rounded-full h-12 w-12 border-t-4 border-b-4 border-indigo-500 mx-auto"></div>
                <p className="text-lg font-medium">Loading, please wait...</p>
            </div>
        </div>
    );
}
export default function ErrorPage({ exception }) {
    return (
        <div className="flex justify-center items-center min-h-screen bg-gradient-to-r from-red-700 via-red-800 to-red-900 text-white">
            <div className="text-center space-y-3">
                <div className="text-4xl font-bold">⚠️ Error</div>
                <p className="text-lg">{exception || 'Something went wrong.'}</p>
            </div>
        </div>
    );
}
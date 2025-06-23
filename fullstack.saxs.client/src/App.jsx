import { useEffect, useState } from "react";
import './App.css';
import CreateSysForm from "./CreateSysForm";
import SysView from "./SysView";

function App() {
    const [systemId, setSystemId] = useState("");
    const [inputId, setInputId] = useState("");
    const [data, setData] = useState(null);

    useEffect(() => {
        fetch("/ping-auth", {
            credentials: "include",
        }).then(res => {
            if (res.status === 401) {
                window.location.href = `https://localhost:7135/Identity/Account/Login`;
            }
        });
    }, []);

    const handleLoadSystem = () => {
        if (!inputId) return;
        fetch(`/api/saxs/sys/get/${inputId}`)
            .then((res) => res.json())
            .then((data) => {
                setSystemId(inputId);
                setData(data);
            })
            .catch(console.error);
    };

    return (
        <div>
            <CreateSysForm />

            <div style={{ margin: '1rem' }}>
                <input
                    type="text"
                    placeholder="Put SYSTEM_ID"
                    value={inputId}
                    onChange={(e) => setInputId(e.target.value)}
                    style={{ padding: '0.5rem', marginRight: '0.5rem' }}
                />
                <button onClick={handleLoadSystem}>Load</button>
            </div>

            {data && (
                <div style={{ height: '100vh' }}>
                    <SysView data={data} />
                </div>
            )}
        </div>
    );
}

export default App;

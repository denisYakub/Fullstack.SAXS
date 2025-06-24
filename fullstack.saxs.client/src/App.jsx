import { useEffect } from "react";
import './App.css';
import {
    BrowserRouter as Router,
    Routes,
    Route,
    Link,
    Navigate
} from "react-router-dom";
import CreateSysForm from "./CreateSysForm";
import DrawSysForm from "./DrawSysForm";
import Draw2Particles from "./Draw2Particles";

function App() {
    useEffect(() => {
        fetch("/ping-auth", {
            credentials: "include",
        }).then(res => {
            if (res.status === 401) {
                window.location.href = `https://localhost:7135/Identity/Account/Login`;
            }
        });
    }, []);

    return (
        <Router>
            <nav style={{ marginBottom: "20px" }}>
                <Link to="/create" style={{ marginRight: "10px" }}>Create system</Link>
                <Link to="/draw" style={{ marginRight: "10px" }}>Draw system</Link>
                <Link to="/test2Particles" style={{ marginRight: "10px" }}>Draw 2 particle to test</Link>
            </nav>
            <Routes>
                <Route path="/" element={<Navigate to="/create" />} />
                <Route path="/create" element={<CreateSysForm />} />
                <Route path="/draw" element={<DrawSysForm />} />
                <Route path="/test2Particles" element={<Draw2Particles /> } />
            </Routes>
        </Router>
    );
}

export default App;

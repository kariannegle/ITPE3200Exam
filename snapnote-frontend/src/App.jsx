import 'bootstrap/dist/css/bootstrap.min.css';
import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import CustomNavbar from './components/CustomNavbar';
import Home from './pages/Home';
import PostList from './pages/PostList';
import Register from './pages/Register';
import Login from './pages/Login';
import Logout from './pages/Logout';


function App() {
  return (
    <Router>
      <CustomNavbar />
      <div className="container" style={{ marginTop: '70px' }}>
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/posts" element={<PostList />} />
          <Route path="/register" element={<Register />} />
          <Route path="/login" element={<Login />} />
          <Route path="/logout" element={<Logout />} />
        </Routes>
      </div>
    </Router>
  );
}

export default App;

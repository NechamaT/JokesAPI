import React from 'react';
import {Route} from 'react-router';
import Layout from './components/Layout';
import {AuthContextComponent} from './AuthContext';
import Signup from './Pages/Signup';
import Login from './Pages/Login';
import Logout from './Pages/Logout';
import PrivateRoute from './PrivateRoute';
import Home from './Pages/Home';
import ViewAll from './Pages/ViewAll';

const App =() =>{
    return(
<AuthContextComponent>
    <Layout>
        <Route exact path='/' component={Home}/>
        <Route exact path='/signup' component={Signup} />
        <Route exact path='/logout' component={Logout} />
        <Route exact path='/login' component={Login} />
        <PrivateRoute exact path='/viewall' component={ViewAll}/>
    </Layout>
</AuthContextComponent>
    )
}
export default App;
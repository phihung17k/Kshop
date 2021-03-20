import Container from '@material-ui/core/Container';
import CssBaseline from '@material-ui/core/CssBaseline';
import Typography from '@material-ui/core/Typography';
import React from 'react';
import Carousels from './Components/Carousel/Caroursel';
import Filter from './Components/Filter/Filter';
const Home = props => {
    const style = {
        homeBody: {
            margin: '0 40px 0 40px',
        }
    }
    return (
        <div>
            <div style={{ height: '80px' }}></div>
            <React.Fragment>
                <CssBaseline />
                <Container maxWidth="xl">
                    <Typography component="div" style={style.homeBody} >
                        <Carousels></Carousels>
                        <Filter></Filter>
                          {/* <div style={{ backgroundColor: '#000', height: '600px', }}></div> */}
                    </Typography>
                </Container>
            </React.Fragment>
        </div>
    )
}

Home.propTypes = {

}

export default Home

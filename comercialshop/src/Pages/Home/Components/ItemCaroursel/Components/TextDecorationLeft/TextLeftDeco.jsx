import Box from '@material-ui/core/Box';
import Typography from '@material-ui/core/Typography';
import React from 'react';
import ColorButton from '../ColorButton/ColorButton';
import Container from '@material-ui/core/Container';


const TextLeftDeco = props => {
    const style = {
        firstText: {
            fontWeight: '100',
            fontSize: '40px',
            height: '70px'
        },
        secondText: {
            fontWeight: 'bold',
            fontSize: '60px',
            height: '90px'
        },
        thirdText: {
            fontWeight: '100',
            fontSize: '20px',
            height: '80px'
        }
    }
    return (
        <Container align="left" maxWidth="sm" style={{ marginLeft: '100px' }} >
            <Typography variant="overline" display="block" style={style.firstText}>
                WHAT IS YOUR
        </Typography>
            <Typography component="div">
                <Box letterSpacing={6} style={style.secondText}>
                    SUMMER HUE ?
            </Box>
            </Typography>
            <Typography component="div">
                <Box letterSpacing={11} style={style.thirdText}>
                    SUMMER 2020 COLLECTION
            </Box>
            </Typography>
            <ColorButton />
        </Container>
    )
}

TextLeftDeco.propTypes = {

}

export default TextLeftDeco
